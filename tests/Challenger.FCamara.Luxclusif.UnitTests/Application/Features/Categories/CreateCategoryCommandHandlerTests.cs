using Challenger.FCamara.Luxclusif.Application.Features.Categories;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.UnitTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.UnitTests.Application.Features.Categories;

public class CreateCategoryCommandHandlerTests : DatabaseTestBase
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateCategorySuccessfully()
    {
        // Arrange
        var handler = new CreateCategoryCommandHandler(Context);
        var command = new CreateCategoryCommand("Electronics", "ELEC", null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().NotBeEmpty();
        result.Data.Name.Should().Be("Electronics");
        result.Data.Shortcode.Should().Be("ELEC");
        result.Data.ParentCategoryId.Should().BeNull();
        result.Message.Should().Be("Categoria criada com sucesso");

        // Verifica se foi salvo no banco
        var savedCategory = await Context.Categories.FindAsync(result.Data.Id);
        savedCategory.Should().NotBeNull();
        savedCategory!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task Handle_WithParentCategoryId_ShouldCreateCategoryWithParent()
    {
        // Arrange
        var parentCategory = Category.Create("Electronics", "ELEC", null);
        Context.Categories.Add(parentCategory);
        await Context.SaveChangesAsync();

        var handler = new CreateCategoryCommandHandler(Context);
        var command = new CreateCategoryCommand("Electronics", "ELEC", parentCategory.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.ParentCategoryId.Should().Be(parentCategory.Id);

        var savedCategory = await Context.Categories.FindAsync(result.Data.Id);
        savedCategory!.ParentCategoryId.Should().Be(parentCategory.Id);
    }

    [Fact]
    public async Task Handle_WithMultipleCategories_ShouldCreateAll()
    {
        // Arrange
        var handler = new CreateCategoryCommandHandler(Context);
        var command1 = new CreateCategoryCommand("Electronics", "ELEC", null);
        var command2 = new CreateCategoryCommand("Electronics", "ELEC", null);

        // Act
        await handler.Handle(command1, CancellationToken.None);
        await handler.Handle(command2, CancellationToken.None);

        // Assert
        var categories = Context.Categories.ToList();
        categories.Should().HaveCount(2);
        categories.Should().Contain(c => c.Name == "Electronics");
        categories.Should().Contain(c => c.Name == "Books");
    }
}