using MediatR;
using ZenitWpp.Application.Contacts.DTOs;

namespace ZenitWpp.Application.Contacts.Commands.CreateContact;

public record CreateContactCommand(
    string Name,
    string PhoneNumber,
    string? Email,
    string? Segment
) : IRequest<ContactResponse>;
