namespace Octopath_Traveler.Models;

public record DamageEvent : CombatEvent
{
    public required int Damage { get; init; }

    public AttackType? Type { get; init; }

    public bool TargetWasDefending { get; init; }
    public bool ExploitedWeakness { get; init; }
    public bool CausedBreak { get; init; }

    public override void Accept(ICombatEventVisitor visitor)
        => visitor.Visit(this);
}
