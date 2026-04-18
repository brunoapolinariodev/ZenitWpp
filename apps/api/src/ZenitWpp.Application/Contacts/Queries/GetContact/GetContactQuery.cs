using MediatR;
using ZenitWpp.Application.Contacts.DTOs;

namespace ZenitWpp.Application.Contacts.Queries.GetContact;

public record GetContactQuery(Guid Id) : IRequest<ContactResponse>;
