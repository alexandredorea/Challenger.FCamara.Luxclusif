using Challenger.FCamara.Luxclusif.Application.Features.Suppliers;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;
using Challenger.FCamara.Luxclusif.UnitTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.UnitTests.Application.Features.Suppliers;

public class GetSupplierByIdQueryHandlerTests : DatabaseTestBase
{
    [Fact]
    public async Task Handle_WithExistingSupplier_ShouldReturnSupplierDetails()
    {
        // Arrange
        var handler = new GetSupplierByIdQueryHandler(Context);
        var supplier = new Supplier(
            "Tech Suppliers Inc",
            "contact@techsuppliers.com",
            Currency.USD,
            "United States");

        Context.Suppliers.Add(supplier);
        await Context.SaveChangesAsync();

        var query = new GetSupplierByIdQuery(supplier.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(supplier.Id);
        result.Data.Name.Should().Be("Tech Suppliers Inc");
        result.Data.Email.Should().Be("contact@techsuppliers.com");
        result.Data.Currency.Should().Be("USD");
        result.Data.Country.Should().Be("United States");
        result.Message.Should().Be("Fornecedor recuperado com sucesso");
    }

    [Fact]
    public async Task Handle_WithNonExistentSupplier_ShouldReturnNotFoundError()
    {
        // Arrange
        var handler = new GetSupplierByIdQueryHandler(Context);
        var nonExistentId = Guid.NewGuid();
        var query = new GetSupplierByIdQuery(nonExistentId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Error[0].Code.Should().Be("NOT_FOUND");
        result.Error[0].Message.Should().Be("Fornecedor não encontrado");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithMultipleSuppliersInDatabase_ShouldReturnCorrectOne()
    {
        // Arrange
        var handler = new GetSupplierByIdQueryHandler(Context);

        var supplier1 = new Supplier("Supplier 1", "s1@test.com", Currency.USD, "USA");
        var supplier2 = new Supplier("Supplier 2", "s2@test.com", Currency.EUR, "Germany");
        var supplier3 = new Supplier("Supplier 3", "s3@test.com", Currency.BRL, "Brazil");

        Context.Suppliers.AddRange(supplier1, supplier2, supplier3);
        await Context.SaveChangesAsync();

        var query = new GetSupplierByIdQuery(supplier2.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Id.Should().Be(supplier2.Id);
        result.Data.Name.Should().Be("Supplier 2");
        result.Data.Email.Should().Be("s2@test.com");
    }

    [Fact]
    public async Task Handle_AfterSupplierDeleted_ShouldReturnNotFound()
    {
        // Arrange
        var handler = new GetSupplierByIdQueryHandler(Context);
        var supplier = new Supplier("Temp Supplier", "temp@test.com", Currency.USD, "USA");

        Context.Suppliers.Add(supplier);
        await Context.SaveChangesAsync();

        var supplierId = supplier.Id;

        // Remove o fornecedor
        Context.Suppliers.Remove(supplier);
        await Context.SaveChangesAsync();

        var query = new GetSupplierByIdQuery(supplierId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error[0].Code.Should().Be("NOT_FOUND");
    }
}