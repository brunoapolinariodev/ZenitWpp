namespace ZenitWpp.Application.Auth.DTOs;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    bool RequiresTwoFactor,
    Guid AgentId,
    string AgentName,
    string Role
);
