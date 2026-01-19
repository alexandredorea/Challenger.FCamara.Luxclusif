using Challenger.FCamara.Luxclusif.Domain.Entities;

namespace Challenger.FCamara.Luxclusif.Application.DTOs;

public record CreateProductResponse(
    Guid Id,
    Guid SupplierId,
    Guid CategoryId,
    string Description,
    decimal AcquisitionCostInSupplierCurrency,
    decimal AcquisitionCostInUSD,
    DateTime AcquireDate,
    string Status,
    string? WmsProductId
)
{
    public static implicit operator CreateProductResponse(Product product)
    {
        return new CreateProductResponse(
            product.Id,
            product.SupplierId,
            product.CategoryId,
            product.Description,
            product.AcquisitionCostInSupplierCurrency,
            product.AcquisitionCostInUSD,
            product.AcquireDate,
            product.Status.ToString(),
            product.WmsProductId);
    }
}