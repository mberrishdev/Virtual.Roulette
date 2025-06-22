namespace Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;

/// <summary>
/// Request model for user registration.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// The desired username for the new user.
    /// </summary>
    /// <example>jane.doe</example>
    public required string Username { get; set; }

    /// <summary>
    /// The password for the new user.
    /// </summary>
    /// <example>NewP@ssword123!</example>
    public required string Password { get; set; }
} 