namespace Octopath_Traveler.Models;

public static class DamageCalculator
{
    private const int MinimumDamage = 0;

    // El truncado va al final: (atk * mod - def) * multiplicador, y recién ahí floor.
    public static int Calculate(Unit attacker, Unit target, Attack attack)
        => Math.Max(MinimumDamage, (int)Math.Floor(
            BaseDamage(attacker, target, attack) * target.DamageMultiplierFor(attack.Type)));

    private static double BaseDamage(Unit attacker, Unit target, Attack attack)
        => attacker.OffensiveStat(attack.Type.Category) * attack.Modifier
           - target.DefensiveStat(attack.Type.Category);
}
