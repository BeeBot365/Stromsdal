namespace StromsdalKok.Domain.Entities;

public class RefreshToken : BaseDomain
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
}