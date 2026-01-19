using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Categories;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID da categoria é obrigatório")
            .NotEqual(Guid.Empty).WithMessage("O ID da categoria deve ser um GUID válido")
            .MustAsync(MustNotHaveProducts).WithMessage("Não é possível excluir categoria com produtos associados")
            .MustAsync(MustNotHaveSubCategories).WithMessage("Não é possível excluir categoria com subcategorias");
    }

    // Verificar se NÃO tem subcategorias
    private async Task<bool> MustNotHaveSubCategories(Guid categoryId, CancellationToken cancellationToken)
    {
        var hasSubCategories = await _context.Categories
            .AnyAsync(c => c.ParentCategoryId == categoryId, cancellationToken);

        return !hasSubCategories;
    }

    // Verificar se NÃO tem produtos associados
    private async Task<bool> MustNotHaveProducts(Guid categoryId, CancellationToken cancellationToken)
    {
        var hasProducts = await _context.Products
            .AnyAsync(p => p.CategoryId == categoryId, cancellationToken);

        return !hasProducts;
    }
}