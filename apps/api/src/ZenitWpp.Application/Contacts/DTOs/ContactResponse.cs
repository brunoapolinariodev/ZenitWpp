namespace ZenitWpp.Application.Contacts.DTOs;

public record ContactResponse(
    Guid Id,
    string Name,
    string PhoneNumber,
    string? Email,
    string? Segment,
    string Language,
    DateTime CreatedAt
);
