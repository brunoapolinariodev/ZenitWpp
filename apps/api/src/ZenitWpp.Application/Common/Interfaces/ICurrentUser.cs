namespace ZenitWpp.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid AgentId { get; }
    string Email { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
}
