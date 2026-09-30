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
        => use.User.Hit(target, CreateAttack(use));

    protected virtual Attack CreateAttack(SkillUse use)
        => new Attack(_type, _modifier);
}
