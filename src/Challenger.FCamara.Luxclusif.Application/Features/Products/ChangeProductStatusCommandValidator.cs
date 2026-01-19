using Challenger.FCamara.Luxclusif.Domain.Enumerators;
using FluentValidation;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public class ChangeProductStatusCommandValidator : AbstractValidator<ChangeProductStatusCommand>
{
    public ChangeProductStatusCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("O ID do produto é obrigatório");

        RuleFor(x => x.NewStatus)
            .IsInEnum()
            .WithMessage("Status de produto inválido");

        RuleFor(x => x.NewStatus)
            .NotEqual(ProductStatus.Created)
            .WithMessage("Não é possível definir manualmente o status como Criado");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("O ID do usuário é obrigatório");

        RuleFor(x => x.UserEmail)
            .NotEmpty()
            .WithMessage("O e-mail do usuário é obrigatório")
            .EmailAddress()
            .WithMessage("Formato de e-mail inválido");
    }
}