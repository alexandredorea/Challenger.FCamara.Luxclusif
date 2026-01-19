using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public sealed class GetProductsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    public async Task<Result<PagedResult<ProductDto>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProductDto(
                p.Id,
                p.Supplier.Name,
                p.Category.Name,
                p.Description,
                p.AcquisitionCostInSupplierCurrency,
                p.AcquisitionCostInUSD,
                p.AcquireDate,
                p.Status.ToString(),
                p.WmsProductId
            )).ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return Result<PagedResult<ProductDto>>.Ok(result!);
    }
}