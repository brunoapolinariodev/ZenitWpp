using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenitWpp.Application.Agents.Commands.CreateAgent;
using ZenitWpp.Application.Agents.Commands.UpdateAgentStatus;
using ZenitWpp.Application.Agents.Queries.GetAgent;
using ZenitWpp.Api.Requests.Agents;

namespace ZenitWpp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/agents")]
public class AgentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AgentController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _mediator.Send(new GetAgentQuery(id)));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAgentRequest request)
    {
        var result = await _mediator.Send(
            new CreateAgentCommand(request.Name, request.Email, request.Password, request.Role));
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAgentStatusRequest request)
    {
        await _mediator.Send(new UpdateAgentStatusCommand(id, request.IsOnline));
        return NoContent();
    }
}
