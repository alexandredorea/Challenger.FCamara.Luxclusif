using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public sealed class ChangeProductStatusCommandHandler(
    IApplicationDbContext context,
    IWmsService wmsService,
    IAuditService auditService,
    IEmailService emailService) : ICommandHandler<ChangeProductStatusCommand, ChangeProductStatusResponse>
{
    public async Task<Result<ChangeProductStatusResponse>> Handle(
        ChangeProductStatusCommand request,
        CancellationToken cancellationToken)
    {
        // Buscar o produto
        var product = await context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result<ChangeProductStatusResponse>.Fail(
                "Produto não encontrado.",
                "NOT_FOUND");
        }

        var previousStatus = product.Status;

        // Aplicar a mudança de status baseada nas regras de negócio
        switch (request.NewStatus)
        {
            case ProductStatus.Sold:
                product.MarkAsSold();
                break;

            case ProductStatus.Cancelled:
                product.MarkAsCancelled();
                break;

            case ProductStatus.Returned:
                product.MarkAsReturned();
                break;

            default:
                return Result<ChangeProductStatusResponse>.Fail(
                    $"Não é possível fazer a transição para o status {request.NewStatus}", "INVALID_STATUS_TRANSITION");
        }

        await context.SaveChangesAsync(cancellationToken);

        // Integrações específicas quando o produto é vendido
        if (request.NewStatus == ProductStatus.Sold)
            await HandleProductSoldIntegrations(product, cancellationToken);

        // Criar log de auditoria para mudança de status
        await CreateProductStatusChanged(request, previousStatus, cancellationToken);

        var response = new ChangeProductStatusResponse(
            product.Id,
            previousStatus,
            product.Status,
            DateTime.UtcNow);

        return Result<ChangeProductStatusResponse>.Ok(
            response,
            $"Status do produto alterado de {previousStatus} para {product.Status}");
    }

    private async Task CreateProductStatusChanged(ChangeProductStatusCommand request, ProductStatus previousStatus, CancellationToken cancellationToken)
    {
        var auditRequest = new AuditLogRequest(
            request.UserId,
            request.UserEmail,
            $"PRODUCT_STATUS_CHANGED_{previousStatus}_TO_{request.NewStatus}",
            DateTime.UtcNow);

        await auditService.LogAsync(auditRequest, cancellationToken);
    }

    private async Task HandleProductSoldIntegrations(
        Product product,
        CancellationToken cancellationToken)
    {
        // Integração 1: Enviar email para o fornecedor
        var emailSubject = $"Product Sold - {product.Description}";
        var emailBody = $@"
                Hello,

                We would like to inform you that the following product has been sold:

                Product ID: {product.Id}
                Description: {product.Description}
                Acquisition Cost: {product.AcquisitionCostInSupplierCurrency} ({product.Supplier.Currency})
                Sold Date: {product.SoldDate:yyyy-MM-dd HH:mm:ss}

                Best regards,
                Inventory Management System
            ";

        await emailService.SendEmailAsync(
            product.Supplier.Email.Value,
            emailSubject,
            emailBody,
            cancellationToken);

        // Integração 2: Trigger dispatch no WMS
        if (!string.IsNullOrEmpty(product.WmsProductId))
            await wmsService.DispatchProductAsync(product.WmsProductId, cancellationToken);
    }
}