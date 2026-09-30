namespace Octopath_Traveler.Models.Skills;

public class MercyStrikeEffect : DamageEffect
{
    public MercyStrikeEffect(AttackType type, double modifier) : base(type, modifier) { }

    protected override Attack CreateAttack(SkillUse use)
        => base.CreateAttack(use).ThatCannotKill();
}
