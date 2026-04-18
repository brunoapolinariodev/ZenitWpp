using MediatR;
using ZenitWpp.Application.Automation.DTOs;

namespace ZenitWpp.Application.Automation.Queries.ListFlows;

public record ListFlowsQuery : IRequest<IEnumerable<FlowResponse>>;
