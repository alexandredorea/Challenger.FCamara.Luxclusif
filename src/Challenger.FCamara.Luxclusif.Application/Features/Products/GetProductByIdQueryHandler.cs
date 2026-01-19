using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public class GetProductByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetProductByIdQuery, ProductDetailDto>
{
    public async Task<Result<ProductDetailDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
        {
            return Result<ProductDetailDto>.Fail(
                "Produto não encontrado.",
                "NOT_FOUND");
        }

        var supplierInfo = new SupplierInfo(
            product.Supplier.Id,
            product.Supplier.Name,
            product.Supplier.Email.Value,
            product.Supplier.Currency.ToString());

        var categoryInfo = new CategoryInfo(
            product.Category.Id,
            product.Category.Name,
            product.Category.Shortcode);

        var productDto = new ProductDetailDto(
            product.Id,
            supplierInfo,
            categoryInfo,
            product.Description,
            product.AcquisitionCostInSupplierCurrency,
            product.AcquisitionCostInUSD,
            product.AcquireDate,
            product.SoldDate,
            product.CancelDate,
            product.ReturnDate,
            product.Status.ToString(),
            product.WmsProductId,
            product.CreatedAt,
            product.UpdatedAt);

        return Result<ProductDetailDto>.Ok(
            productDto,
            "Produto recuperado com sucesso");
    }
}