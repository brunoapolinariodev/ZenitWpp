using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenitWpp.Application.Conversations.Commands.CloseConversation;
using ZenitWpp.Application.Conversations.Commands.CreateConversation;
using ZenitWpp.Application.Conversations.Commands.SendMessage;
using ZenitWpp.Application.Conversations.Commands.TransferConversation;
using ZenitWpp.Application.Conversations.Commands.UploadMedia;
using ZenitWpp.Application.Conversations.Queries.GetConversation;
using ZenitWpp.Application.Conversations.Queries.ListConversations;
using ZenitWpp.Api.Requests.Conversations;

namespace ZenitWpp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/conversations")]
public class ConversationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new ListConversationsQuery(page, pageSize)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _mediator.Send(new GetConversationQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConversationRequest request)
    {
        var result = await _mediator.Send(new CreateConversationCommand(request.ContactId, request.Channel));
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/messages")]
    public async Task<IActionResult> SendMessage(Guid id, [FromBody] SendMessageRequest request)
        => Ok(await _mediator.Send(new SendMessageCommand(id, request.Content, request.Type, request.Direction)));

    [HttpPost("{id:guid}/messages/media")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadMedia(Guid id, [FromForm] UploadMediaRequest request)
    {
        var file = request.File;
        await using var stream = file.OpenReadStream();
        var result = await _mediator.Send(new UploadMediaCommand(
            id, stream, file.FileName, file.ContentType, request.Direction));
        return Ok(result);
    }

    [HttpPatch("{id:guid}/transfer")]
    public async Task<IActionResult> Transfer(Guid id, [FromBody] TransferConversationRequest request)
        => Ok(await _mediator.Send(new TransferConversationCommand(id, request.NewAgentId)));

    [HttpPatch("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id)
    {
        await _mediator.Send(new CloseConversationCommand(id));
        return NoContent();
    }
}
