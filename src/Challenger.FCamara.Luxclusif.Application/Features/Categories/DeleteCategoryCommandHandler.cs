using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Categories;

public sealed class DeleteCategoryCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category is null)
            return Result.Fail("Categoria não encontrada", "NOT_FOUND");

        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok("Categoria removida com sucesso");
    }
}