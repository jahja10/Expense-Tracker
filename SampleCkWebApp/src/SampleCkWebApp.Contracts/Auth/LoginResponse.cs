namespace SampleCkWebApp.Contracts.Auth;

public sealed class LoginResponse
{
    public int UserId { get; init; }
    public string Email { get; init; } = null!;
    public string Token { get; init; } = null!;
}
