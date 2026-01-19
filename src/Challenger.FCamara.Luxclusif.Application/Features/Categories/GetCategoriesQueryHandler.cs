using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Categories;

public sealed class GetCategoriesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetCategoriesQuery, PagedResult<CategoryDto>>
{
    public async Task<Result<PagedResult<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Categories
            .Include(c => c.ParentCategory)
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Shortcode,
                c.ParentCategoryId,
                c.ParentCategory.Name,
                c.CreatedAt)
            ).ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken)
            //.ContinueWith(t => t.Result.Map(p => (CategoryDto?)p), cancellationToken)
            ;

        return Result<PagedResult<CategoryDto>>.Ok(result!);
    }
}