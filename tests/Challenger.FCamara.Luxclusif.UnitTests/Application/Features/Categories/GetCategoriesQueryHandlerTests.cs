using Challenger.FCamara.Luxclusif.Application.Features.Categories;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.UnitTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.UnitTests.Application.Features.Categories;

public class GetCategoriesQueryHandlerTests : DatabaseTestBase
{
    private readonly GetCategoriesQueryHandler _handler;

    public GetCategoriesQueryHandlerTests()
    {
        _handler = new GetCategoriesQueryHandler(Context);
    }

    [Fact]
    public async Task Handle_WithExistingCategories_ShouldReturnPagedResult()
    {
        // Arrange
        var parentCategory = Category.Create("Electronics", "ELEC", null);
        var childCategory = Category.Create("Smartphones", "SMART", parentCategory.Id);

        Context.Categories.AddRange(parentCategory, childCategory);
        await Context.SaveChangesAsync();

        var query = new GetCategoriesQuery(1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Items.Should().HaveCount(2);
        result.Data.TotalCount.Should().Be(2);
        result.Data.Page.Should().Be(1);
        result.Data.PageSize.Should().Be(10);
        result.Data.Items.Should().BeInAscendingOrder(x => x.Name);
    }

    [Fact]
    public async Task Handle_WithEmptyDatabase_ShouldReturnEmptyPagedResult()
    {
        // Arrange
        var query = new GetCategoriesQuery(1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Items.Should().BeEmpty();
        result.Data.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var categories = Enumerable.Range(1, 25)
            .Select(i => Category.Create($"Category {i:D2}", $"CAT{i:D2}", null))
            .ToList();

        Context.Categories.AddRange(categories);
        await Context.SaveChangesAsync();

        var query = new GetCategoriesQuery(2, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Items.Should().HaveCount(10);
        result.Data.TotalCount.Should().Be(25);
        result.Data.Page.Should().Be(2);
        result.Data.TotalPages.Should().Be(3);
    }

    [Fact]
    public async Task Handle_ShouldIncludeParentCategoryName()
    {
        // Arrange
        var parentCategory = Category.Create("Electronics", "ELEC", null);
        Context.Categories.Add(parentCategory);
        await Context.SaveChangesAsync();

        var childCategory = Category.Create("Smartphones", "SMART", parentCategory.Id);
        Context.Categories.Add(childCategory);
        await Context.SaveChangesAsync();

        var query = new GetCategoriesQuery(1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Items.Should().Contain(c =>
            c.Name == "Smartphones" && c.ParentCategoryName == "Electronics");
    }

    [Fact]
    public async Task Handle_ShouldOrderCategoriesByName()
    {
        // Arrange
        Context.Categories.AddRange(
            Category.Create("Zebra", "ZEB", null),
            Category.Create("Apple", "APP", null),
            Category.Create("Mango", "MAN", null)
        );
        await Context.SaveChangesAsync();

        var query = new GetCategoriesQuery(1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Items.Select(x => x.Name).Should().BeInAscendingOrder();
        result.Data.Items.First().Name.Should().Be("Apple");
        result.Data.Items.Last().Name.Should().Be("Zebra");
    }
}