using Mapster;
using MediatR;
using ZenitWpp.Application.Contacts.DTOs;
using ZenitWpp.Domain.Contacts.Repositories;

namespace ZenitWpp.Application.Contacts.Queries.GetContact;

public class GetContactHandler : IRequestHandler<GetContactQuery, ContactResponse>
{
    private readonly IContactRepository _repository;

    public GetContactHandler(IContactRepository repository)
        => _repository = repository;

    public async Task<ContactResponse> Handle(GetContactQuery query, CancellationToken ct)
    {
        var contact = await _repository.GetByIdAsync(query.Id, ct)
            ?? throw new KeyNotFoundException($"Contato {query.Id} não encontrado.");

        return contact.Adapt<ContactResponse>();
    }
}
