namespace Octopath_Traveler.Models;

public record SpRecoveryEvent : CombatEvent
{
    public required int Amount { get; init; }

    public override bool ChangedHp => false;

    public override void Accept(ICombatEventVisitor visitor)
        => visitor.Visit(this);
}
