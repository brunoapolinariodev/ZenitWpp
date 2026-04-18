using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenitWpp.Application.Campaigns.Commands.CreateCampaign;
using ZenitWpp.Application.Campaigns.Commands.SendCampaign;
using ZenitWpp.Application.Campaigns.Queries.GetCampaign;
using ZenitWpp.Api.Requests.Campaigns;

namespace ZenitWpp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/campaigns")]
public class CampaignController : ControllerBase
{
    private readonly IMediator _mediator;

    public CampaignController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _mediator.Send(new GetCampaignQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCampaignRequest request)
    {
        var result = await _mediator.Send(
            new CreateCampaignCommand(request.Name, request.Message, request.Segment, request.ScheduledAt));
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/send")]
    public async Task<IActionResult> Send(Guid id)
    {
        await _mediator.Send(new SendCampaignCommand(id));
        return Accepted();
    }
}
