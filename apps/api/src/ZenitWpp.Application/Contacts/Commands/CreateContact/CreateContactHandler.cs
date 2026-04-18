using Mapster;
using MediatR;
using ZenitWpp.Application.Contacts.DTOs;
using ZenitWpp.Domain.Contacts;
using ZenitWpp.Domain.Contacts.Repositories;

namespace ZenitWpp.Application.Contacts.Commands.CreateContact;

public class CreateContactHandler : IRequestHandler<CreateContactCommand, ContactResponse>
{
    private readonly IContactRepository _repository;

    public CreateContactHandler(IContactRepository repository)
        => _repository = repository;

    public async Task<ContactResponse> Handle(CreateContactCommand cmd, CancellationToken ct)
    {
        var contact = Contact.Create(cmd.Name, cmd.PhoneNumber, cmd.Email, cmd.Segment);
        await _repository.AddAsync(contact, ct);
        return contact.Adapt<ContactResponse>();
    }
}
