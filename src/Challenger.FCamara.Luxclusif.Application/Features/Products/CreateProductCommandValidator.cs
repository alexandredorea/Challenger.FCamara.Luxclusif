using FluentValidation;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("O ID do fornecedor é obrigatório");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("O ID da categoria é obrigatório");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória")
            .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres");

        RuleFor(x => x.AcquisitionCostInSupplierCurrency)
            .GreaterThan(0).WithMessage("O custo de aquisição na moeda do fornecedor deve ser maior que zero");

        RuleFor(x => x.AcquisitionCostInUSD)
            .GreaterThan(0).WithMessage("O custo de aquisição em USD deve ser maior que zero");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório");

        When(x => string.IsNullOrWhiteSpace(x.UserEmail), () =>
        {
            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("O e-mail do usuário é obrigatório");
        }).Otherwise(() =>
        {
            RuleFor(x => x.UserEmail)
                .EmailAddress().WithMessage("Formato de e-mail inválido");
        });
    }
}