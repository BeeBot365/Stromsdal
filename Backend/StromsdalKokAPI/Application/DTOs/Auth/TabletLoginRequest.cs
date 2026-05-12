namespace StromsdalKok.Application.DTOs.Auth;

public class TabletLoginRequest
{
    public required string DeviceName { get; set; }
    public required string Password { get; set; }
}