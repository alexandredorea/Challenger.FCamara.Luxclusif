using FluentValidation;

namespace Challenger.FCamara.Luxclusif.Application.Features.Suppliers;

public sealed class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do fornecedor é obrigatório.")
            .MaximumLength(200).WithMessage("O nome do fornecedor não pode exceder 200 caracteres.");

        When(x => string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.");
        }).Otherwise(() =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("E-mail com formato inválido")
                .MaximumLength(254).WithMessage("E-mail não pode exceder 254 caracteres");
        });

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Moeda inválida");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("O país é obrigatório")
            .MaximumLength(100).WithMessage("O país não pode exceder 100 caracteres.");
    }
}