using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;
using Challenger.FCamara.Luxclusif.Application.Features.Categories;
using Challenger.FCamara.Luxclusif.Application.Features.Suppliers;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using MediatR;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public class CreateProductCommandHandler(
    IMediator mediator,
    IApplicationDbContext context,
    IWmsService wmsService,
    IAuditService auditService) : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<Result<CreateProductResponse>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = await mediator.Send(new GetSupplierByIdQuery(request.SupplierId), cancellationToken);
        if (!supplier.Success)
            return Result<CreateProductResponse>.Fail(supplier.Error[0].Message, supplier.Error[0].Code);

        var category = await mediator.Send(new GetCategoryByIdQuery(request.CategoryId), cancellationToken);
        if (!category.Success)
            return Result<CreateProductResponse>.Fail(category.Error[0].Message, category.Error[0].Code);

        // Criar o produto
        var product = await CreateProductAsync(request, cancellationToken);

        // Integração 1: Criar produto no WMS
        var wmsResponse = await CreateProductWMSAsync(
            product.Id.ToString(),
            product.Description,
            category.Data!.Shortcode,
            product.SupplierId.ToString(),
            cancellationToken);

        product.SetWmsProductId(wmsResponse.WmsProductId);
        await context.SaveChangesAsync(cancellationToken);

        // Integração 2: Criar log de auditoria
        await CreateAuditLogAsync(request, cancellationToken);

        return Result<CreateProductResponse>.Ok(product, "Produto criado com sucesso");
    }

    private async Task CreateAuditLogAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var auditRequest = new AuditLogRequest(
            request.UserId,
            request.UserEmail,
            "PRODUCT_CREATED",
            DateTime.UtcNow);

        await auditService.LogAsync(auditRequest, cancellationToken);
    }

    private async Task<Product> CreateProductAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(
            request.SupplierId,
            request.CategoryId,
            request.Description,
            request.AcquisitionCostInSupplierCurrency,
            request.AcquisitionCostInUSD);

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
        return product;
    }

    private async Task<WmsProductResponse> CreateProductWMSAsync(string productId, string productDescription, string shortcode, string supplierId, CancellationToken cancellationToken)
    {
        var wmsRequest = new WmsProductRequest(
            productId,
            productDescription,
            shortcode,
            supplierId);

        var wmsResponse = await wmsService.CreateProductAsync(wmsRequest, cancellationToken);

        return wmsResponse;
    }
}