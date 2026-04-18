namespace ZenitWpp.Api.Requests.Auth;

public record LoginRequest(string Email, string Password, string? TotpCode);
