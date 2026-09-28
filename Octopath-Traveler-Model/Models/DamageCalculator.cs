namespace Octopath_Traveler.Models;

public static class DamageCalculator
{
    private const int MinimumDamage = 0;
    private const int MinimumSurvivingHp = 1;

    public static int Calculate(Unit attacker, Unit target, Attack attack)
        => Capped(RawDamage(attacker, target, attack), target, attack);

    // El truncado va al final: (atk * mod - def) * bonus * multiplicador, y recién ahí floor.
    private static int RawDamage(Unit attacker, Unit target, Attack attack)
        => Math.Max(MinimumDamage, (int)Math.Floor(BaseDamage(attacker, target, attack)
            * attack.Bonus * target.DamageMultiplierFor(attack.Type)));

    private static double BaseDamage(Unit attacker, Unit target, Attack attack)
        => attacker.OffensiveStat(attack.Type.Category) * attack.Modifier
           - target.DefensiveStat(attack.Type.Category);

    private static int Capped(int damage, Unit target, Attack attack)
        => attack.CanKill ? damage : Math.Min(damage, target.CurrentHp - MinimumSurvivingHp);
}
