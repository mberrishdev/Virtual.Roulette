namespace Virtual.Roulette.Application.Contracts.Services.AuthServices.Models;

public record JwtTokenResult(string Token, DateTime ExpiresAt);