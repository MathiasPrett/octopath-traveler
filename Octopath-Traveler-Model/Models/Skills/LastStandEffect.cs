namespace Octopath_Traveler.Models.Skills;

public class LastStandEffect : DamageEffect
{
    private const double BonusPerMissingPercent = 0.03;
    private const int FullHpPercent = 100;

    public LastStandEffect(AttackType type, double modifier) : base(type, modifier) { }

    protected override Attack CreateAttack(SkillParameters parameters)
        => base.CreateAttack(parameters).CopyWithBonus(CalculateBonus(parameters.User));

    private static double CalculateBonus(Unit user)
        => 1 + BonusPerMissingPercent * CalculateMissingHpPercent(user);

    private static int CalculateMissingHpPercent(Unit user)
        => (int)Math.Floor((double)(user.MaxHp - user.CurrentHp) / user.MaxHp * FullHpPercent);
}
