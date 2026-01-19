using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Suppliers;

public sealed class GetSuppliersQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSuppliersQuery, PagedResult<SupplierDto>>
{
    public async Task<Result<PagedResult<SupplierDto>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Suppliers
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Select(c => new SupplierDto(
                c.Id,
                c.Name,
                c.Email.Value,
                c.Currency,
                c.Country,
                c.CreatedAt)
            ).ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken)
            //.ContinueWith(t => t.Result.Map(p => (CategoryDto?)p), cancellationToken)
            ;

        return Result<PagedResult<SupplierDto>>.Ok(result!);
    }
}