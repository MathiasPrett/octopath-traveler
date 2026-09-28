namespace Octopath_Traveler.Models;

public record HitResult
{
    public required Unit Target { get; init; }
    public required int Damage { get; init; }
    public required AttackType Type { get; init; }
    public bool TargetWasDefending { get; init; }
    public bool ExploitedWeakness { get; init; }
    public bool CausedBreak { get; init; }
}
