using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Automation;

public class FlowStep : BaseEntity
{
    public Guid FlowId { get; private set; }
    public StepType Type { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public Guid? NextStepId { get; private set; }

    private FlowStep() { }

    internal FlowStep(Guid flowId, StepType type, string content, int order, Guid? nextStepId = null)
    {
        FlowId = flowId;
        Type = type;
        Content = content;
        Order = order;
        NextStepId = nextStepId;
    }
}
