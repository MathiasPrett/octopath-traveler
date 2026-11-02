namespace Octopath_Traveler.Models;

public record HealEvent : CombatEvent
{
    public required int Amount { get; init; }

    public override void Accept(ICombatEventVisitor visitor)
        => visitor.Visit(this);
}
