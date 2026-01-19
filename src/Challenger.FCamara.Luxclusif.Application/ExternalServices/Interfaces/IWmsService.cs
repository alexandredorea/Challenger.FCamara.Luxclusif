namespace Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;

public interface IWmsService
{
    Task<WmsProductResponse> CreateProductAsync(
        WmsProductRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DispatchProductAsync(
        string wmsProductId,
        CancellationToken cancellationToken = default);
}

public record WmsProductRequest(
    string ProductId,
    string Description,
    string CategoryShortcode,
    string SupplierId
);

public record WmsProductResponse(
    string WmsProductId
);