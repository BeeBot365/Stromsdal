namespace StromsdalKok.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("E-post får inte vara tom.");

        if (!email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException($"Ogiltig e-postadress: {email}");

        return new Email(email.ToLowerInvariant().Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}