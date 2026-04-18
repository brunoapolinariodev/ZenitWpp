using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenitWpp.Application.Contacts.Commands.CreateContact;
using ZenitWpp.Application.Contacts.Commands.UpdateContact;
using ZenitWpp.Application.Contacts.Queries.GetContact;
using ZenitWpp.Api.Requests.Contacts;

namespace ZenitWpp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/contacts")]
public class ContactController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _mediator.Send(new GetContactQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
    {
        var result = await _mediator.Send(
            new CreateContactCommand(request.Name, request.PhoneNumber, request.Email, request.Segment));
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactRequest request)
        => Ok(await _mediator.Send(new UpdateContactCommand(id, request.Name, request.Email, request.Segment)));
}
