using MediatR;
using ZenitWpp.Application.Auth.DTOs;
using ZenitWpp.Application.Common.Interfaces;
using ZenitWpp.Domain.Agents.Repositories;

namespace ZenitWpp.Application.Auth.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IAgentRepository _agents;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtService _jwt;
    private readonly ITotpService _totp;

    public LoginHandler(
        IAgentRepository agents,
        IPasswordHasher hasher,
        IJwtService jwt,
        ITotpService totp)
    {
        _agents = agents;
        _hasher = hasher;
        _jwt = jwt;
        _totp = totp;
    }

    public async Task<AuthResponse> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var agent = await _agents.GetByEmailAsync(cmd.Email.Trim().ToLower(), ct)
            ?? throw new UnauthorizedAccessException("Credenciais inválidas.");

        if (!_hasher.Verify(cmd.Password, agent.PasswordHash))
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        if (agent.TwoFactorEnabled)
        {
            if (string.IsNullOrWhiteSpace(cmd.TotpCode))
                return new AuthResponse(string.Empty, string.Empty, true, agent.Id, agent.Name, agent.Role.ToString());

            if (!_totp.ValidateCode(agent.TwoFactorSecret!, cmd.TotpCode))
                throw new UnauthorizedAccessException("Código 2FA inválido.");
        }

        var accessToken = _jwt.GenerateToken(agent);
        var refreshToken = _jwt.GenerateRefreshToken();

        return new AuthResponse(accessToken, refreshToken, false, agent.Id, agent.Name, agent.Role.ToString());
    }
}
