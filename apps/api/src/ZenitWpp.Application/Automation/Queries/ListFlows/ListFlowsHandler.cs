using Mapster;
using MediatR;
using ZenitWpp.Application.Automation.DTOs;
using ZenitWpp.Domain.Automation.Repositories;

namespace ZenitWpp.Application.Automation.Queries.ListFlows;

public class ListFlowsHandler : IRequestHandler<ListFlowsQuery, IEnumerable<FlowResponse>>
{
    private readonly IFlowRepository _repository;

    public ListFlowsHandler(IFlowRepository repository) => _repository = repository;

    public async Task<IEnumerable<FlowResponse>> Handle(ListFlowsQuery _, CancellationToken ct)
    {
        var flows = await _repository.ListActiveAsync(ct);
        return flows.Adapt<IEnumerable<FlowResponse>>();
    }
}
