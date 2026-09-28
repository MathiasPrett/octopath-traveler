namespace Octopath_Traveler.Models.Skills;

public class DamageEffect : Effect
{
    private readonly AttackType _type;
    private readonly double _modifier;

    public DamageEffect(AttackType type, double modifier)
    {
        _type = type;
        _modifier = modifier;
    }

    public override CombatEvent Apply(SkillUse use, Unit target)
        => use.User.Hit(target, AttackFor(use));

    protected virtual Attack AttackFor(SkillUse use)
        => new Attack(_type, _modifier);
}
