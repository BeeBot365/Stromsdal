namespace StromsdalKok.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "SEK")
    {
        if (amount < 0)
            throw new ArgumentException("Belopp kan inte vara negativt.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Valuta måste anges.");

        return new Money(Math.Round(amount, 2), currency.ToUpperInvariant());
    }

    public static Money Zero(string currency = "SEK") => new(0, currency);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Kan inte addera olika valutor.");
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Kan inte subtrahera olika valutor.");
        return new Money(Amount - other.Amount, Currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount.ToString(GetCurrencyDecimals(Currency))} {Currency}";

    private string GetCurrencyDecimals(string currency) => currency switch
    {
        "SEK" => "F2",
        _ => "F0"
    };
}