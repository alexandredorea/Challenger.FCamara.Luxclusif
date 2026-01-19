using Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace Challenger.FCamara.Luxclusif.Infrastructure.ExternalServices;

public class AuditService(HttpClient httpClient, ILogger<AuditService> logger) : IAuditService
{
    public async Task<bool> LogAsync(
        AuditLogRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation(
                "Creating audit log: UserId={UserId}, Email={Email}, Action={ActionName}",
                request.UserId,
                request.Email,
                request.ActionName);

            // Mock: Simular chamada HTTP
            // Em produção, seria:
            // var response = await _httpClient.PostAsJsonAsync("/logs", request, cancellationToken);
            // response.EnsureSuccessStatusCode();

            await Task.Delay(50, cancellationToken); // Simular latência de rede

            logger.LogInformation(
                "Audit log created successfully: Action={ActionName}",
                request.ActionName);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating audit log");
            // Não falhar a operação principal por erro no audit
            return false;
        }
    }
}