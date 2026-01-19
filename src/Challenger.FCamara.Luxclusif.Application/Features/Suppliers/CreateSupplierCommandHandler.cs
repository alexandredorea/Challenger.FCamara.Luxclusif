using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Suppliers;

public sealed class CreateSupplierCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateSupplierCommand, CreateSupplierResponse>
{
    public async Task<Result<CreateSupplierResponse>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.ToLowerInvariant().Trim();
        var entity = await context.Suppliers.AnyAsync(s => s.Email.Value == normalizedEmail, cancellationToken);

        if (entity)
            return Result<CreateSupplierResponse>.Fail("Já existe um fornecedor com este endereço de e-mail.", "CONFLICT");

        var supplier = new Supplier(
                request.Name,
                request.Email,
                request.Currency,
                request.Country);

        context.Suppliers.Add(supplier);
        await context.SaveChangesAsync(cancellationToken);

        return Result<CreateSupplierResponse>.Ok(supplier, "Fornecedor criado com sucesso.");
    }
}