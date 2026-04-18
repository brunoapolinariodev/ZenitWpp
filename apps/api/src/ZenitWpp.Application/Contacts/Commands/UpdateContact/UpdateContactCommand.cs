using MediatR;
using ZenitWpp.Application.Contacts.DTOs;

namespace ZenitWpp.Application.Contacts.Commands.UpdateContact;

public record UpdateContactCommand(Guid Id, string Name, string? Email, string? Segment) : IRequest<ContactResponse>;
