namespace Octopath_Traveler.Models.Skills;

// Mercy Strike: el objetivo no puede bajar de 1 HP.
public class MercyStrikeEffect : DamageEffect
{
    public MercyStrikeEffect(AttackType type, double modifier) : base(type, modifier) { }

    protected override Attack AttackFor(SkillUse use)
        => base.AttackFor(use).ThatCannotKill();
}
