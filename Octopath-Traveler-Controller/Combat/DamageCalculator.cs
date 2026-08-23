using Octopath_Traveler.Models;

namespace Octopath_Traveler.Combat;

public static class DamageCalculator
{
    private const double BasicAttackModifier = 1.3;
    private const int MinimumDamage = 0;

    public static int Calculate(Unit attacker, Unit defender)
        => Math.Max(MinimumDamage, (int)Math.Floor(
            attacker.Stats.PhysAtk * BasicAttackModifier - defender.Stats.PhysDef));
}
