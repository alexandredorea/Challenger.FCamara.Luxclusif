using Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace Challenger.FCamara.Luxclusif.Infrastructure.ExternalServices;

public class WmsService(HttpClient httpClient, ILogger<WmsService> logger) : IWmsService
{
    public async Task<WmsProductResponse> CreateProductAsync(
        WmsProductRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation(
                "Creating product in WMS: ProductId={ProductId}, Description={Description}",
                request.ProductId,
                request.Description);

            // Mock: Simular chamada HTTP
            // Em produção, seria algo mais ou menos assim:
            // var response = await _httpClient.PostAsJsonAsync("/products", request, cancellationToken);
            // response.EnsureSuccessStatusCode();
            // var result = await response.Content.ReadFromJsonAsync<WmsProductResponse>();

            // Mock response
            await Task.Delay(100, cancellationToken); // Simular latência de rede

            var wmsProductId = $"WMS-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";

            logger.LogInformation(
                "Product created in WMS successfully: WmsProductId={WmsProductId}",
                wmsProductId);

            return new WmsProductResponse(wmsProductId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating product in WMS");
            throw new InvalidOperationException("Failed to create product in WMS", ex);
        }
    }

    public async Task<bool> DispatchProductAsync(
        string wmsProductId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation(
                "Dispatching product in WMS: WmsProductId={WmsProductId}",
                wmsProductId);

            // Mock: Simular chamada HTTP
            // Em produção, seria:
            // var response = await _httpClient.PostAsync($"/products/{wmsProductId}/dispatch", null, cancellationToken);
            // response.EnsureSuccessStatusCode();

            await Task.Delay(100, cancellationToken); // Simular latência de rede

            logger.LogInformation(
                "Product dispatched in WMS successfully: WmsProductId={WmsProductId}",
                wmsProductId);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error dispatching product in WMS");
            throw new InvalidOperationException("Failed to dispatch product in WMS", ex);
        }
    }
}