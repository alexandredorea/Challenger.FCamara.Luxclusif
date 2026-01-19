using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;

namespace Challenger.FCamara.Luxclusif.Application.Features.Suppliers;

public sealed record CreateSupplierCommand(
    string Name,
    string Email,
    Currency Currency,
    string Country
) : ICommand<CreateSupplierResponse>;

public sealed record CreateSupplierResponse(
    Guid Id,
    string Name,
    string Email,
    Currency Currency,
    string Country
)
{
    public static implicit operator CreateSupplierResponse(Supplier supplier)
    {
        return new CreateSupplierResponse(
            supplier.Id,
            supplier.Name,
            supplier.Email.Value,
            supplier.Currency,
            supplier.Country);
    }
}