using Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Challenger.FCamara.Luxclusif.Infrastructure.ExternalServices;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(
                "Sending email: To={To}, Subject={Subject}",
                to,
                subject);

            // Mock: Simular envio de email
            // Em produção, integraria com SMTP, SendGrid, etc.

            await Task.Delay(100, cancellationToken); // Simular latência

            _logger.LogInformation(
                "Email sent successfully to {To}",
                to);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {To}", to);
            return false;
        }
    }
}