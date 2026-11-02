namespace Octopath_Traveler.Models;

public record TrapEvent : CombatEvent
{
    public required int Rounds { get; init; }

    public override bool ChangedHp => false;

    public override void Accept(ICombatEventVisitor visitor)
        => visitor.Visit(this);
}
