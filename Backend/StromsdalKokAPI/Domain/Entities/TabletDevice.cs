using System.Text.RegularExpressions;
using BC = BCrypt.Net.BCrypt;

namespace StromsdalKok.Domain.Entities;

public class TabletDevice : BaseDomain
{
    private const int DeviceNameMinLength = 2;
    private const int DeviceNameMaxLength = 50;
    public string DeviceName { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public string? Token { get; private set; }
    public DateTime? TokenExpireAt { get; private set; }

    internal TabletDevice(int id, string deviceName, bool isActive, string password, string? token, DateTime? expireAt)
    {
        Id = id;
        DeviceName = deviceName;
        Password = password;
        IsActive = isActive;
        Token = token;
        TokenExpireAt = expireAt;
    }

    private TabletDevice(string deviceName, string password)
    {
        DeviceName = deviceName;
        Password = password;
        IsActive = true;
    }

    public static TabletDevice Create(string deviceName, string password)
    {
        if (string.IsNullOrWhiteSpace(deviceName))
            throw new ArgumentException("Enhetsnamn får inte vara tomt.");

        if (deviceName.Length < DeviceNameMinLength)
            throw new ArgumentException($"Enhetsnamn måste vara minst {DeviceNameMinLength} tecken.");

        if (deviceName.Length > DeviceNameMaxLength)
            throw new ArgumentException($"Enhetsnamn får inte vara längre än {DeviceNameMaxLength} tecken.");

        if (!Regex.IsMatch(deviceName, @"^[a-zA-ZåäöÅÄÖ0-9\s\-]+$"))
            throw new ArgumentException("Enhetsnamn får endast innehålla bokstäver, siffror, mellanslag och bindestreck.");

        return new TabletDevice(deviceName, BC.HashPassword(password));
    }
    public void Deactivate() => IsActive = false;
}