namespace Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
} 