namespace Octopath_Traveler.Models;

public record ReviveEvent : CombatEvent
{
    public override void Accept(ICombatEventVisitor visitor)
        => visitor.Visit(this);
}
