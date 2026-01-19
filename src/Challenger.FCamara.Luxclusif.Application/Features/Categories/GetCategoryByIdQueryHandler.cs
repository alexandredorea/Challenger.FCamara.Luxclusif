using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Categories;

public sealed class GetCategoryByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.categoryId, cancellationToken);

        if (result is null)
            return Result<CategoryDto>.Fail(
                "Categoria não encontrada",
                "NOT_FOUND");

        return Result<CategoryDto>.Ok(
            result,
            "Categoria recuperada com sucesso");
    }
}