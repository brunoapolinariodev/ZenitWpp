using Mapster;
using MediatR;
using ZenitWpp.Application.Automation.DTOs;
using ZenitWpp.Domain.Automation;
using ZenitWpp.Domain.Automation.Repositories;

namespace ZenitWpp.Application.Automation.Commands.CreateFlow;

public class CreateFlowHandler : IRequestHandler<CreateFlowCommand, FlowResponse>
{
    private readonly IFlowRepository _repository;

    public CreateFlowHandler(IFlowRepository repository) => _repository = repository;

    public async Task<FlowResponse> Handle(CreateFlowCommand cmd, CancellationToken ct)
    {
        var flow = Flow.Create(cmd.Name, cmd.TriggerType, cmd.TriggerValue);
        await _repository.AddAsync(flow, ct);
        return flow.Adapt<FlowResponse>();
    }
}
