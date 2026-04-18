namespace ZenitWpp.Api.Requests.Contacts;

public record CreateContactRequest(string Name, string PhoneNumber, string? Email, string? Segment);
