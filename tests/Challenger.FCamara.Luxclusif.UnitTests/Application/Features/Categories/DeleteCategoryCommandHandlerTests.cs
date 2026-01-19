using Challenger.FCamara.Luxclusif.Application.Features.Categories;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.UnitTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.UnitTests.Application.Features.Categories;

public sealed class DeleteCategoryCommandHandlerTests : DatabaseTestBase
{
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategoryCommandHandlerTests()
    {
        _handler = new DeleteCategoryCommandHandler(Context);
    }

    [Fact]
    public async Task Handle_WithExistingCategory_ShouldDeleteSuccessfully()
    {
        // Arrange
        var category = Category.Create("Electronics", "ELEC", null);
        Context.Categories.Add(category);
        await Context.SaveChangesAsync();

        var command = new DeleteCategoryCommand(category.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Categoria removida com sucesso");

        // Verifica se foi removida do banco
        var deletedCategory = await Context.Categories.FindAsync(category.Id);
        deletedCategory.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithNonExistentCategory_ShouldReturnNotFoundError()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var command = new DeleteCategoryCommand(nonExistentId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle();
        result.Error[0].Code.Should().Be("NOT_FOUND");
        result.Error[0].Message.Should().Be("Categoria não encontrada");

        // Verifica que nenhuma categoria foi removida
        var categoriesCount = Context.Categories.Count();
        categoriesCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WithCategoryThatHasChildren_ShouldStillDelete()
    {
        // Arrange
        var parentCategory = Category.Create("Electronics", "ELEC", null);
        Context.Categories.Add(parentCategory);
        await Context.SaveChangesAsync();

        var childCategory = Category.Create("Smartphones", "SMART", parentCategory.Id);
        Context.Categories.Add(childCategory);
        await Context.SaveChangesAsync();

        var command = new DeleteCategoryCommand(parentCategory.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();

        var deletedParent = await Context.Categories.FindAsync(parentCategory.Id);
        deletedParent.Should().BeNull();
    }
}