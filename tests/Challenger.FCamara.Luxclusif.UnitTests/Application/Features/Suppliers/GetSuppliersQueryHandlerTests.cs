using Challenger.FCamara.Luxclusif.Application.Features.Suppliers;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;
using Challenger.FCamara.Luxclusif.UnitTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.UnitTests.Application.Features.Suppliers
{
    public class GetSuppliersQueryHandlerTests : DatabaseTestBase
    {
        [Fact]
        public async Task Handle_WithExistingSuppliers_ShouldReturnPagedResult()
        {
            // Arrange
            var handler = new GetSuppliersQueryHandler(Context);

            var supplier1 = new Supplier("Alpha Suppliers", "alpha@test.com", Currency.USD, "USA");
            var supplier2 = new Supplier("Beta Suppliers", "beta@test.com", Currency.EUR, "Germany");
            var supplier3 = new Supplier("Gamma Suppliers", "gamma@test.com", Currency.BRL, "Brazil");

            Context.Suppliers.AddRange(supplier1, supplier2, supplier3);
            await Context.SaveChangesAsync();

            var query = new GetSuppliersQuery(1, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Items.Should().HaveCount(3);
            result.Data.TotalCount.Should().Be(3);
            result.Data.Page.Should().Be(1);
            result.Data.PageSize.Should().Be(10);
            result.Data.TotalPages.Should().Be(1);
        }

        [Fact]
        public async Task Handle_WithEmptyDatabase_ShouldReturnEmptyPagedResult()
        {
            // Arrange
            var handler = new GetSuppliersQueryHandler(Context);
            var query = new GetSuppliersQuery(1, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Items.Should().BeEmpty();
            result.Data.TotalCount.Should().Be(0);
            result.Data.Page.Should().Be(1);
            result.Data.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task Handle_ShouldOrderSuppliersByName()
        {
            // Arrange
            var handler = new GetSuppliersQueryHandler(Context);

            Context.Suppliers.AddRange(
                new Supplier("Zebra Suppliers", "zebra@test.com", Currency.USD, "USA"),
                new Supplier("Alpha Suppliers", "alpha@test.com", Currency.EUR, "Germany"),
                new Supplier("Mango Suppliers", "mango@test.com", Currency.BRL, "Brazil")
            );
            await Context.SaveChangesAsync();

            var query = new GetSuppliersQuery(1, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.Items.Select(x => x.Name).Should().BeInAscendingOrder();
            result.Data.Items.First().Name.Should().Be("Alpha Suppliers");
            result.Data.Items.Last().Name.Should().Be("Zebra Suppliers");
        }

        [Fact]
        public async Task Handle_WithPagination_ShouldReturnCorrectPage()
        {
            // Arrange
            var handler = new GetSuppliersQueryHandler(Context);

            var suppliers = Enumerable.Range(1, 25)
                .Select(i => new Supplier(
                    $"Supplier {i:D2}",
                    $"supplier{i}@test.com",
                    Currency.USD,
                    "USA"))
                .ToList();

            Context.Suppliers.AddRange(suppliers);
            await Context.SaveChangesAsync();

            var query = new GetSuppliersQuery(2, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Items.Should().HaveCount(10);
            result.Data.TotalCount.Should().Be(25);
            result.Data.Page.Should().Be(2);
            result.Data.PageSize.Should().Be(10);
            result.Data.TotalPages.Should().Be(3);
        }

        [Fact]
        public async Task Handle_WithLastPagePartiallyFilled_ShouldReturnRemainingItems()
        {
            // Arrange
            var handler = new GetSuppliersQueryHandler(Context);

            var suppliers = Enumerable.Range(1, 23)
                .Select(i => new Supplier(
                    $"Supplier {i:D2}",
                    $"supplier{i}@test.com",
                    Currency.USD,
                    "USA"))
                .ToList();

            Context.Suppliers.AddRange(suppliers);
            await Context.SaveChangesAsync();

            var query = new GetSuppliersQuery(3, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.Items.Should().HaveCount(3); // Última página com apenas 3 itens
            result.Data.TotalCount.Should().Be(23);
            result.Data.Page.Should().Be(3);
            result.Data.TotalPages.Should().Be(3);
        }

        [Fact]
        public async Task Handle_ShouldIncludeAllSupplierProperties()
        {
            // Arrange
            var handler = new GetSuppliersQueryHandler(Context);
            var supplier = new Supplier(
                "Complete Supplier",
                "complete@test.com",
                Currency.BRL,
                "Brazil");

            Context.Suppliers.Add(supplier);
            await Context.SaveChangesAsync();

            var query = new GetSuppliersQuery(1, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var dto = result.Data.Items.Single();
            dto.Id.Should().Be(supplier.Id);
            dto.Name.Should().Be("Complete Supplier");
            dto.Email.Should().Be("complete@test.com");
            dto.Currency.Should().Be(Currency.BRL);
            dto.Country.Should().Be("Brazil");
            dto.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(2, 5)]
        [InlineData(1, 10)]
        [InlineData(3, 3)]
        public async Task Handle_WithDifferentPageSizes_ShouldReturnCorrectResults(
            int page,
            int pageSize)
        {
            // Arrange
            var handler = new GetSuppliersQueryHandler(Context);

            var suppliers = Enumerable.Range(1, 15)
                .Select(i => new Supplier(
                    $"Supplier {i:D2}",
                    $"supplier{i}@test.com",
                    Currency.USD,
                    "USA"))
                .ToList();

            Context.Suppliers.AddRange(suppliers);
            await Context.SaveChangesAsync();

            var query = new GetSuppliersQuery(page, pageSize);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Page.Should().Be(page);
            result.Data.PageSize.Should().Be(pageSize);
            result.Data.TotalCount.Should().Be(15);
            result.Data.Items.Should().HaveCountLessThanOrEqualTo(pageSize);
        }

        [Fact]
        public async Task Handle_WithDifferentCurrenciesAndCountries_ShouldReturnAll()
        {
            // Arrange
            var handler = new GetSuppliersQueryHandler(Context);

            Context.Suppliers.AddRange(
                new Supplier("US Supplier", "us@test.com", Currency.USD, "United States"),
                new Supplier("EU Supplier", "eu@test.com", Currency.EUR, "Germany"),
                new Supplier("BR Supplier", "br@test.com", Currency.BRL, "Brazil"),
                new Supplier("UK Supplier", "uk@test.com", Currency.GBP, "United Kingdom")
            );
            await Context.SaveChangesAsync();

            var query = new GetSuppliersQuery(1, 10);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.Items.Should().HaveCount(4);
            result.Data.Items.Should().Contain(s => s.Currency == Currency.USD);
            result.Data.Items.Should().Contain(s => s.Currency == Currency.EUR);
            result.Data.Items.Should().Contain(s => s.Currency == Currency.BRL);
            result.Data.Items.Should().Contain(s => s.Currency == Currency.GBP);
        }
    }
}