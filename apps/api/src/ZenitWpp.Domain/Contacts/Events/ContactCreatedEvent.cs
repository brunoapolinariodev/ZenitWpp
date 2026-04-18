using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Contacts.Events;

public class ContactCreatedEvent : BaseDomainEvent
{
    public Guid ContactId { get; }
    public string Name { get; }
    public string PhoneNumber { get; }

    public ContactCreatedEvent(Guid contactId, string name, string phoneNumber)
    {
        ContactId = contactId;
        Name = name;
        PhoneNumber = phoneNumber;
    }
}
