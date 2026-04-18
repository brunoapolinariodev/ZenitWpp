using MediatR;
using ZenitWpp.Application.Reports.DTOs;
using ZenitWpp.Domain.Conversations;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Application.Reports.Queries.GetDashboard;

public class GetDashboardHandler : IRequestHandler<GetDashboardQuery, DashboardResponse>
{
    private readonly IConversationRepository _conversations;

    public GetDashboardHandler(IConversationRepository conversations)
        => _conversations = conversations;

    public async Task<DashboardResponse> Handle(GetDashboardQuery query, CancellationToken ct)
    {
        var all = (await _conversations.ListAsync(1, int.MaxValue, ct)).ToList();

        return new DashboardResponse(
            TotalConversations: all.Count,
            OpenConversations: all.Count(c => c.Status == ConversationStatus.Open),
            InProgressConversations: all.Count(c => c.Status == ConversationStatus.InProgress),
            ClosedConversations: all.Count(c => c.Status == ConversationStatus.Closed),
            GeneratedAt: DateTime.UtcNow
        );
    }
}
