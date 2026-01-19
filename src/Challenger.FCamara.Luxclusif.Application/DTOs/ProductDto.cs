namespace Challenger.FCamara.Luxclusif.Application.DTOs;

public sealed record ProductDto(
    Guid Id,
    string SupplierName,
    string CategoryName,
    string Description,
    decimal AcquisitionCostInSupplierCurrency,
    decimal AcquisitionCostInUSD,
    DateTime AcquireDate,
    string Status,
    string? WmsProductId
);