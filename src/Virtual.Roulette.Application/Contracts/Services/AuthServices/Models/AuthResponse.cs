namespace Virtual.Roulette.Application.Contracts.Services.AuthService.Models;

public sealed record AuthResponse(
    long UserId,
    string UserName,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string DialCode,
    string Email,
    string? ProfilePicture,
    DateTime Expires,
    string Token,
    string RefreshToken,
    AuthStatus AuthStatus = AuthStatus.Authorized,
    string? AccessToken = null,
    List<string> Roles = null!)
{
    public AuthStatus AuthStatus { get; set; } = AuthStatus.Authorized;
}

public enum AuthStatus
{
    Authorized = 1,
    NeedCompleteRegistration = 2,
}