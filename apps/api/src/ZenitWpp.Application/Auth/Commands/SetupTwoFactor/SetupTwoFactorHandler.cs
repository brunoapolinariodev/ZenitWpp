using MediatR;
using ZenitWpp.Application.Auth.DTOs;
using ZenitWpp.Application.Common.Interfaces;
using ZenitWpp.Domain.Agents.Repositories;

namespace ZenitWpp.Application.Auth.Commands.SetupTwoFactor;

public class SetupTwoFactorHandler : IRequestHandler<SetupTwoFactorCommand, TwoFactorSetupResponse>
{
    private readonly IAgentRepository _agents;
    private readonly ITotpService _totp;

    public SetupTwoFactorHandler(IAgentRepository agents, ITotpService totp)
    {
        _agents = agents;
        _totp = totp;
    }

    public async Task<TwoFactorSetupResponse> Handle(SetupTwoFactorCommand cmd, CancellationToken ct)
    {
        var agent = await _agents.GetByIdAsync(cmd.AgentId, ct)
            ?? throw new KeyNotFoundException($"Agente {cmd.AgentId} não encontrado.");

        var secret = _totp.GenerateSecret();
        var qrUri = _totp.GenerateQrCodeUri(agent.Email, secret);

        agent.EnableTwoFactor(secret);
        await _agents.UpdateAsync(agent, ct);

        return new TwoFactorSetupResponse(secret, qrUri);
    }
}
