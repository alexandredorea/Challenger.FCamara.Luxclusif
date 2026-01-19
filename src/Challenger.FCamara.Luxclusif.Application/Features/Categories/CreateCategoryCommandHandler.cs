using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Challenger.FCamara.Luxclusif.Domain.Entities;

namespace Challenger.FCamara.Luxclusif.Application.Features.Categories;

public sealed class CreateCategoryCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Category.Create(
            request.Name,
            request.Shortcode,
            request.ParentCategoryId);

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return Result<CreateCategoryResponse>.Ok(category, "Categoria criada com sucesso");
    }
}