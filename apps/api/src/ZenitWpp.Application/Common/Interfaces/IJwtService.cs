using ZenitWpp.Domain.Agents;

namespace ZenitWpp.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(Agent agent);
    string GenerateRefreshToken();
}
