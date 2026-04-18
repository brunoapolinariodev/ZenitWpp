using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ZenitWpp.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task JoinConversation(string conversationId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

    public async Task LeaveConversation(string conversationId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);

    public async Task NotifyTyping(string conversationId)
        => await Clients.OthersInGroup(conversationId).SendAsync("AgentTyping", Context.UserIdentifier);
}
