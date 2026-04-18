using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenitWpp.Application.Reports.Queries.GetDashboard;

namespace ZenitWpp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator) => _mediator = mediator;

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
        => Ok(await _mediator.Send(new GetDashboardQuery()));
}
