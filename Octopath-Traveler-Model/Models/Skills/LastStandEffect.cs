namespace Octopath_Traveler.Models.Skills;

// Last Stand: 3% más de daño por cada 1% de HP que le falte al usuario. El porcentaje
// faltante se trunca a entero antes de multiplicar.
public class LastStandEffect : DamageEffect
{
    private const double BonusPerMissingPercent = 0.03;
    private const int FullHpPercent = 100;

    public LastStandEffect(AttackType type, double modifier) : base(type, modifier) { }

    protected override Attack AttackFor(SkillUse use)
        => base.AttackFor(use).WithBonus(BonusFor(use.User));

    private static double BonusFor(Unit user)
        => 1 + BonusPerMissingPercent * MissingHpPercent(user);

    private static int MissingHpPercent(Unit user)
        => (int)Math.Floor((double)(user.MaxHp - user.CurrentHp) / user.MaxHp * FullHpPercent);
}
