using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.DTOs;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public record CreateProductCommand(
    Guid SupplierId,
    Guid CategoryId,
    string Description,
    decimal AcquisitionCostInSupplierCurrency,
    decimal AcquisitionCostInUSD,
    string UserId,
    string UserEmail
) : ICommand<CreateProductResponse>;