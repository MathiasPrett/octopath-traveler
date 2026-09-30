namespace Octopath_Traveler.Models;

public static class DamageCalculator
{
    private const int MinimumDamage = 0;
    private const int MinimumSurvivingHp = 1;

    public static int Calculate(Unit attacker, Unit target, Attack attack)
        => CapDamage(CalculateRawDamage(attacker, target, attack), target, attack);

    private static int CalculateRawDamage(Unit attacker, Unit target, Attack attack)
        => Math.Max(MinimumDamage, (int)Math.Floor(CalculateBaseDamage(attacker, target, attack)
            * attack.Bonus * target.GetDamageMultiplier(attack.Type)));

    private static double CalculateBaseDamage(Unit attacker, Unit target, Attack attack)
        => attacker.GetOffensiveStat(attack.Type.Category) * attack.Modifier
           - target.GetDefensiveStat(attack.Type.Category);

    private static int CapDamage(int damage, Unit target, Attack attack)
        => attack.CanKill ? damage : Math.Min(damage, target.CurrentHp - MinimumSurvivingHp);
}
