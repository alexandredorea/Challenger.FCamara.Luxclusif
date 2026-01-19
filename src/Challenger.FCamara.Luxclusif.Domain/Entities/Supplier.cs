using Challenger.FCamara.Luxclusif.Domain.Entities.Base;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;
using Challenger.FCamara.Luxclusif.Domain.ValueObjects;

namespace Challenger.FCamara.Luxclusif.Domain.Entities;

public sealed class Supplier : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = default!;
    public Currency Currency { get; private set; }
    public string Country { get; private set; } = string.Empty;

    // Navigation properties
    public ICollection<Product> Products { get; private set; }

    private Supplier()
    {
        Products = [];
    }

    public Supplier(string name, string email, Currency currency, string country) : this()
    {
        Name = name;
        Email = Email.Create(email);
        Currency = currency;
        Country = country;
    }

    public void Update(string name, string email, Currency currency, string country)
    {
        Name = name;
        Email = Email = Email.Create(email);
        Currency = currency;
        Country = country;
        SetUpdatedAt();
    }
}