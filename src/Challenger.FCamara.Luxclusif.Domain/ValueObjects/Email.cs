namespace Challenger.FCamara.Luxclusif.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get; private set; } = string.Empty;

    private Email()
    { }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        email = email.ToLowerInvariant().Trim();

        if (!IsValidEmail(email))
            throw new ArgumentException("E-mail com formato inválido", nameof(email));

        return new Email(email);
    }

    public static implicit operator string(Email email) => email.Value;

    public override string ToString() => Value;

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        if (email.Length > 254)
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}