using Challenger.FCamara.Luxclusif.Domain.Entities;

namespace Challenger.FCamara.Luxclusif.Application.DTOs;

public record SupplierDetailDto(
    Guid Id,
    string Name,
    string Email,
    string Currency,
    string Country,
    DateTime CreatedAt,
    DateTime? UpdatedAt
)
{
    public static implicit operator SupplierDetailDto(Supplier supplier)
    {
        return new SupplierDetailDto(
            supplier.Id,
            supplier.Name,
            supplier.Email.Value,
            supplier.Currency.ToString(),
            supplier.Country,
            supplier.CreatedAt,
            supplier.UpdatedAt);
    }
}