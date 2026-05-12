public class LoginTabletRequest
{
    public required string Email { get; set; }
    public string? Password { get; set; }
    public string? PinCode { get; set; }
    public string? FaceRef { get; set; }
}