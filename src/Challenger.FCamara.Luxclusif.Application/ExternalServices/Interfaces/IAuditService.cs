namespace Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;

public interface IAuditService
{
    Task<bool> LogAsync(
        AuditLogRequest request,
        CancellationToken cancellationToken = default);
}

public record AuditLogRequest(
    string UserId,
    string Email,
    string ActionName,
    DateTime Timestamp
);