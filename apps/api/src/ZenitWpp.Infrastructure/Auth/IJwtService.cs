using ZenitWpp.Domain.Agents;

namespace ZenitWpp.Infrastructure.Auth;

public interface IJwtService
{
    string GenerateToken(Agent agent);
    string GenerateRefreshToken();
}
