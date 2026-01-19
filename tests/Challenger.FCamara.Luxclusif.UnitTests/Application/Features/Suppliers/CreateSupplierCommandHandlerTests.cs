using Challenger.FCamara.Luxclusif.Application.Features.Suppliers;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;
using Challenger.FCamara.Luxclusif.UnitTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.UnitTests.Application.Features.Suppliers;

public class CreateSupplierCommandHandlerTests : DatabaseTestBase
{
    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateSupplierSuccessfully()
    {
        // Arrange
        var handler = new CreateSupplierCommandHandler(Context);
        var command = new CreateSupplierCommand("Tech Suppliers Inc", "contact@techsuppliers.com", Currency.USD, "United States");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().NotBeEmpty();
        result.Data.Name.Should().Be("Tech Suppliers Inc");
        result.Data.Email.Should().Be("contact@techsuppliers.com");
        result.Data.Currency.Should().Be(Currency.USD);
        result.Data.Country.Should().Be("United States");
        result.Message.Should().Be("Fornecedor criado com sucesso.");

        // Verifica se foi salvo no banco
        var savedSupplier = await Context.Suppliers.FindAsync(result.Data.Id);
        savedSupplier.Should().NotBeNull();
        savedSupplier!.Name.Should().Be("Tech Suppliers Inc");
        savedSupplier.Email.Value.Should().Be("contact@techsuppliers.com");
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ShouldReturnConflictError()
    {
        // Arrange
        var handler = new CreateSupplierCommandHandler(Context);
        var existingSupplier = new Supplier(
            "Existing Supplier",
            "duplicate@test.com",
            Currency.USD,
            "USA");

        Context.Suppliers.Add(existingSupplier);
        await Context.SaveChangesAsync();

        var command = new CreateSupplierCommand("New Supplier", "duplicate@test.com", Currency.EUR, "France");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Error[0].Code.Should().Be("CONFLICT");
        result.Error[0].Message.Should().Be("Já existe um fornecedor com este endereço de e-mail.");

        // Verifica que apenas um fornecedor existe
        var suppliersCount = await Context.Suppliers.CountAsync();
        suppliersCount.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmailDifferentCase_ShouldReturnConflictError()
    {
        // Arrange
        var handler = new CreateSupplierCommandHandler(Context);
        var existingSupplier = new Supplier(
            "Existing Supplier",
            "test@example.com",
            Currency.USD,
            "USA");

        Context.Suppliers.Add(existingSupplier);
        await Context.SaveChangesAsync();

        var command = new CreateSupplierCommand("New Supplier", "TEST@EXAMPLE.COM", Currency.EUR, "France");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error[0].Code.Should().Be("CONFLICT");
        result.Error[0].Message.Should().Be("Já existe um fornecedor com este endereço de e-mail.");
    }

    [Fact]
    public async Task Handle_WithEmailWithWhitespace_ShouldNormalizeAndCreate()
    {
        // Arrange
        var handler = new CreateSupplierCommandHandler(Context);
        var command = new CreateSupplierCommand("Supplier With Spaces", "  contact@supplier.com  ", Currency.BRL, "Brazil");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();

        var savedSupplier = await Context.Suppliers.FindAsync(result.Data.Id);
        savedSupplier!.Email.Value.Should().Be("contact@supplier.com");
    }

    [Fact]
    public async Task Handle_WithMultipleDifferentSuppliers_ShouldCreateAll()
    {
        // Arrange
        var handler = new CreateSupplierCommandHandler(Context);

        var command1 = new CreateSupplierCommand("Supplier A", "a@supplier.com", Currency.USD, "USA");
        var command2 = new CreateSupplierCommand("Supplier B", "b@supplier.com", Currency.EUR, "Germany");
        var command3 = new CreateSupplierCommand("Supplier C", "c@supplier.com", Currency.BRL, "Brazil");

        // Act
        var result1 = await handler.Handle(command1, CancellationToken.None);
        var result2 = await handler.Handle(command2, CancellationToken.None);
        var result3 = await handler.Handle(command3, CancellationToken.None);

        // Assert
        result1.Success.Should().BeTrue();
        result2.Success.Should().BeTrue();
        result3.Success.Should().BeTrue();

        var suppliers = await Context.Suppliers.ToListAsync();
        suppliers.Should().HaveCount(3);
        suppliers.Should().Contain(s => s.Name == "Supplier A");
        suppliers.Should().Contain(s => s.Name == "Supplier B");
        suppliers.Should().Contain(s => s.Name == "Supplier C");
    }

    [Theory]
    [InlineData(Currency.USD, "United States")]
    [InlineData(Currency.EUR, "Germany")]
    [InlineData(Currency.BRL, "Brazil")]
    [InlineData(Currency.GBP, "United Kingdom")]
    public async Task Handle_WithDifferentCurrenciesAndCountries_ShouldCreateSuccessfully(
        Currency currency,
        string country)
    {
        // Arrange
        var handler = new CreateSupplierCommandHandler(Context);
        var command = new CreateSupplierCommand($"Supplier {currency}", $"contact@{currency}.com".ToLower(), currency, country);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Currency.Should().Be(currency);
        result.Data.Country.Should().Be(country);
    }
}