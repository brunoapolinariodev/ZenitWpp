using MediatR;
using ZenitWpp.Application.Auth.DTOs;

namespace ZenitWpp.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password, string? TotpCode) : IRequest<AuthResponse>;
