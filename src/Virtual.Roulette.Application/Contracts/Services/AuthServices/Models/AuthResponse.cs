namespace Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;

/// <summary>
/// Represents the authentication response returned after a successful login or token refresh.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// ID of the authenticated user.
    /// </summary>
    public required int UserId { get; set; }

    /// <summary>
    /// Username of the authenticated user.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// Access token (JWT) issued to the user.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Expiration time of the access token.
    /// </summary>
    public required DateTime TokenExpiration { get; set; }

    /// <summary>
    /// Refresh token issued to the user for renewing the access token.
    /// </summary>
    public required string RefreshToken { get; set; }
}
