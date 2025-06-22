namespace Virtual.Roulette.Application.Settings;

public class AuthSettings
{
    public int TokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
    public string ValidIssuer { get; set; } = default!;
    public string ValidAudience { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
}