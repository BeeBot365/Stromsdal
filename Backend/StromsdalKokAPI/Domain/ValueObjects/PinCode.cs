namespace StromsdalKok.Domain.ValueObjects;

public sealed class PinCode : ValueObject
{
    public string Value { get; }

    private PinCode(string value) => Value = value;

    public static PinCode Create(string pin)
    {
        if (string.IsNullOrWhiteSpace(pin))
            throw new ArgumentException("PIN-kod får inte vara tom.");

        if (pin.Length < 4 || pin.Length > 6)
            throw new ArgumentException("PIN-kod måste vara 4–6 siffror.");

        if (!pin.All(char.IsDigit))
            throw new ArgumentException("PIN-kod får endast innehålla siffror.");

        return new PinCode(pin);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}