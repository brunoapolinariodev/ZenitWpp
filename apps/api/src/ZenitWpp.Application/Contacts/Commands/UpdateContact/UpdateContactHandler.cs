using Mapster;
using MediatR;
using ZenitWpp.Application.Contacts.DTOs;
using ZenitWpp.Domain.Contacts.Repositories;

namespace ZenitWpp.Application.Contacts.Commands.UpdateContact;

public class UpdateContactHandler : IRequestHandler<UpdateContactCommand, ContactResponse>
{
    private readonly IContactRepository _repository;

    public UpdateContactHandler(IContactRepository repository)
        => _repository = repository;

    public async Task<ContactResponse> Handle(UpdateContactCommand cmd, CancellationToken ct)
    {
        var contact = await _repository.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Contato {cmd.Id} não encontrado.");

        contact.Update(cmd.Name, cmd.Email, cmd.Segment);
        await _repository.UpdateAsync(contact, ct);
        return contact.Adapt<ContactResponse>();
    }
}
