namespace Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken = default);
}