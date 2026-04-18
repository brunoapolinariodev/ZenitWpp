using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Automation;

public class Flow : AggregateRoot
{
    private readonly List<FlowStep> _steps = new();

    public string Name { get; private set; } = string.Empty;
    public TriggerType TriggerType { get; private set; }
    public string? TriggerValue { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<FlowStep> Steps => _steps.AsReadOnly();

    private Flow() { }

    public static Flow Create(string name, TriggerType triggerType, string? triggerValue = null)
    {
        return new Flow
        {
            Name = name,
            TriggerType = triggerType,
            TriggerValue = triggerValue,
            IsActive = true
        };
    }

    public void AddStep(StepType type, string content, int order, Guid? nextStepId = null)
    {
        var step = new FlowStep(Id, type, content, order, nextStepId);
        _steps.Add(step);
        SetUpdatedAt();
    }

    public void Activate() { IsActive = true; SetUpdatedAt(); }
    public void Deactivate() { IsActive = false; SetUpdatedAt(); }
}
