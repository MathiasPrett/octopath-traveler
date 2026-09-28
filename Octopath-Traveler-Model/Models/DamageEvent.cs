namespace Octopath_Traveler.Models;

public record DamageEvent : CombatEvent
{
    public required int Damage { get; init; }

    // Sin tipo cuando el daño no es físico ni elemental (Vortal Claw).
    public AttackType? Type { get; init; }

    public bool TargetWasDefending { get; init; }
    public bool ExploitedWeakness { get; init; }
    public bool CausedBreak { get; init; }
}
