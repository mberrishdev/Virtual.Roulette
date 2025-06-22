namespace Virtual.Roulette.Application.Models;

public class LoginResponse
{
    public required int UserId { get; set; }
    public required string UserName { get; set; }
    public required string Token { get; set; }
    public required DateTime TokenExpiration { get; set; }
    public required string RefreshToken { get; set; }
}