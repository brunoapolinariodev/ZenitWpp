using MediatR;
using ZenitWpp.Application.Auth.DTOs;

namespace ZenitWpp.Application.Auth.Commands.SetupTwoFactor;

public record SetupTwoFactorCommand(Guid AgentId) : IRequest<TwoFactorSetupResponse>;
