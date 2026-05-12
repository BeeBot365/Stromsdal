using StromsdalKok.Domain.ValueObjects;

namespace StromsdalKok.Domain.Entities;

public class Station : BaseDomain
{
    public string Name { get; private set; }
    public Money HourlyCost { get; private set; }
    public bool IsActive { get; private set; }

    internal Station(int id, string name,
    Money hourlyCost, bool isActive)
    {
        Id = id;
        Name = name;
        HourlyCost = hourlyCost;
        IsActive = isActive;
    }

    private Station(string name, Money hourlyCost)
    {
        Name = name;
        HourlyCost = hourlyCost;
        IsActive = true;
    }

    public static Station Create(string name, Money hourlyCost)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Stationsnamn får inte vara tomt.");

        return new Station(name.Trim(), hourlyCost);
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
    public void UpdateCost(Money hourlyCost) => HourlyCost = hourlyCost;
}