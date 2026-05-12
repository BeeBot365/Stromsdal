namespace StromsdalKok.Domain.ValueObjects;

public sealed class FullName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Förnamn får inte vara tomt.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Efternamn får inte vara tomt.");

        string firstLetter = firstName[0].ToString();
        firstName = firstLetter + firstName.Substring(1);
        firstLetter = lastName[0].ToString();
        lastName = firstLetter + lastName.Substring(1);

        return new FullName(
            firstName.Trim(),
            lastName.Trim()
        );
    }

    public string Display => $"{FirstName} {LastName}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString() => Display;
}