namespace Virtual.Roulette.Application.Contracts.Services.AuthService.Models;

/// <summary>
/// Request model for user authentication.
/// </summary>
public class AuthRequest
{
    /// <summary>
    /// The user's username.
    /// </summary>
    /// <example>john.doe</example>
    public required string UserName { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    /// <example>P@ssword123!</example>
    public required string Password { get; set; }
}