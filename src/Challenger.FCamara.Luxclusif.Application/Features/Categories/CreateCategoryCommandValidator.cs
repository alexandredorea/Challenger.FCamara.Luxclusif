using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Categories;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da categoria é obrigatório")
            .MaximumLength(100).WithMessage("O nome da categoria não pode exceder 100 caracteres");

        When(x => string.IsNullOrEmpty(x.Shortcode), () =>
        {
            RuleFor(x => x.Shortcode)
                .NotEmpty().WithMessage("O Shortcode é obrigatório");
        }).Otherwise(() =>
        {
            RuleFor(x => x.Shortcode)
                .MaximumLength(10).WithMessage("O Shortcode não pode exceder 10 caracteres")
                .Matches("^[A-Z0-9]+$").WithMessage("O Shortcode deve conter apenas letras e números")
                .MustAsync(ShortcodeMustBeUnique).WithMessage("Já existe uma categoria com este shortcode");
        });

        When(x => x.ParentCategoryId.HasValue, () =>
        {
            RuleFor(x => x.ParentCategoryId)
                .NotEqual(Guid.Empty).WithMessage("O ID da categoria pai deve ser um GUID válido")
                .MustAsync(ParentCategoryMustExist).WithMessage("Categoria pai não existe");
        });
    }

    // Validar se a categoria pai existe (se fornecida)
    private async Task<bool> ParentCategoryMustExist(Guid? parentCategoryId, CancellationToken cancellationToken)
    {
        if (!parentCategoryId.HasValue)
            return true;

        return await _context.Categories
            .AnyAsync(c => c.Id == parentCategoryId.Value, cancellationToken);
    }

    // Validar se o shortcode já existe
    private async Task<bool> ShortcodeMustBeUnique(string shortcode, CancellationToken cancellationToken)
    {
        var exists = await _context.Categories
            .AnyAsync(c => c.Shortcode == shortcode.ToUpperInvariant(), cancellationToken);

        return !exists;
    }
}