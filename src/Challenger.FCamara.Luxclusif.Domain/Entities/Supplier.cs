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
        ValidateName(name);
        ValidateCountry(country);

        Name = name;
        Email = Email.Create(email);
        Currency = currency;
        Country = country;
    }

    public void Update(string name, string email, Currency currency, string country)
    {
        ValidateName(name);
        ValidateCountry(country);

        Name = name;
        Email = Email = Email.Create(email);
        Currency = currency;
        Country = country;
        SetUpdatedAt();
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do fornecedor não pode estar vazio.", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("O nome do fornecedor não pode exceder 200 caracteres.", nameof(name));
    }

    private static void ValidateCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("O país não pode ficar vazio", nameof(country));

        if (country.Length > 100)
            throw new ArgumentException("O país não pode exceder 100 caracteres.", nameof(country));
    }
}