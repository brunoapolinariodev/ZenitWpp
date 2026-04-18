using ZenitWpp.Domain.Common;
using ZenitWpp.Domain.Contacts.Events;
using ZenitWpp.Domain.Contacts.ValueObjects;

namespace ZenitWpp.Domain.Contacts;

public class Contact : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public Email? Email { get; private set; }
    public string? Segment { get; private set; }
    public string Language { get; private set; } = "pt-BR";

    private Contact() { }

    public static Contact Create(string name, string phoneNumber, string? email = null, string? segment = null)
    {
        var contact = new Contact
        {
            Name = name,
            PhoneNumber = PhoneNumber.Create(phoneNumber),
            Email = email is not null ? Email.Create(email) : null,
            Segment = segment
        };

        contact.AddDomainEvent(new ContactCreatedEvent(contact.Id, name, phoneNumber));
        return contact;
    }

    public void Update(string name, string? email, string? segment)
    {
        Name = name;
        Email = email is not null ? Email.Create(email) : null;
        Segment = segment;
        SetUpdatedAt();
    }

    public void SetLanguage(string language)
    {
        Language = language;
        SetUpdatedAt();
    }
}
