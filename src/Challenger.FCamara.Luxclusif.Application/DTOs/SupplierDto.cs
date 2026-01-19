using Challenger.FCamara.Luxclusif.Domain.Entities;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;

namespace Challenger.FCamara.Luxclusif.Application.DTOs;

public record SupplierDto(
    Guid Id,
    string Name,
    string Email,
    Currency Currency,
    string Country,
    DateTime CreatedAt
)
{
    public static implicit operator SupplierDto(Supplier supplier)
    {
        return new SupplierDto(
            supplier.Id,
            supplier.Name,
            supplier.Email,
            supplier.Currency,
            supplier.Country,
            supplier.CreatedAt);
    }
}