namespace Octopath_Traveler.Models;

public abstract record CombatEvent
{
    public required Unit Target { get; init; }

    public virtual bool ChangedHp => true;

    public abstract void Accept(ICombatEventVisitor visitor);
}
