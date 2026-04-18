namespace ZenitWpp.Application.Reports.DTOs;

public record DashboardResponse(
    int TotalConversations,
    int OpenConversations,
    int InProgressConversations,
    int ClosedConversations,
    DateTime GeneratedAt
);
