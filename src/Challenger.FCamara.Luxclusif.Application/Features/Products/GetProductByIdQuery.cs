using Challenger.FCamara.Luxclusif.Application.Common.CQS;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDetailDto>;

public record ProductDetailDto(
    Guid Id,
    SupplierInfo Supplier,
    CategoryInfo Category,
    string Description,
    decimal AcquisitionCostInSupplierCurrency,
    decimal AcquisitionCostInUSD,
    DateTime AcquireDate,
    DateTime? SoldDate,
    DateTime? CancelDate,
    DateTime? ReturnDate,
    string Status,
    string? WmsProductId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record SupplierInfo(
    Guid Id,
    string Name,
    string Email,
    string Currency
);

public record CategoryInfo(
    Guid Id,
    string Name,
    string Shortcode
);