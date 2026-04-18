using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZenitWpp.Api.Requests.Auth;
using ZenitWpp.Application.Auth.Commands.Login;
using ZenitWpp.Application.Auth.Commands.SetupTwoFactor;

namespace ZenitWpp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender) => _sender = sender;

    /// <summary>Autentica o agente e retorna o token JWT.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var response = await _sender.Send(
            new LoginCommand(request.Email, request.Password, request.TotpCode), ct);

        if (response.RequiresTwoFactor)
            return Ok(new { requiresTwoFactor = true });

        return Ok(response);
    }

    /// <summary>Configura o 2FA (TOTP) para o agente autenticado.</summary>
    [HttpPost("2fa/setup")]
    [Authorize]
    public async Task<IActionResult> SetupTwoFactor(CancellationToken ct)
    {
        var agentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException());

        var response = await _sender.Send(new SetupTwoFactorCommand(agentId), ct);
        return Ok(response);
    }
}
