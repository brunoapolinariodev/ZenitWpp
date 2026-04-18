using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenitWpp.Api.Requests.Automation;
using ZenitWpp.Application.Automation.Commands.CreateFlow;
using ZenitWpp.Application.Automation.Queries.ListFlows;

namespace ZenitWpp.Api.Controllers;

[ApiController]
[Route("api/flows")]
[Authorize]
public class AutomationController : ControllerBase
{
    private readonly ISender _sender;

    public AutomationController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
        => Ok(await _sender.Send(new ListFlowsQuery(), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFlowRequest request, CancellationToken ct)
    {
        var flow = await _sender.Send(
            new CreateFlowCommand(request.Name, request.TriggerType, request.TriggerValue), ct);
        return CreatedAtAction(nameof(List), new { id = flow.Id }, flow);
    }
}
