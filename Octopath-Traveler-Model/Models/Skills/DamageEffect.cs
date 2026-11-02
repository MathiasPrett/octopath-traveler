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

    public override CombatEvent Apply(SkillParameters parameters, Unit target)
        => parameters.User.Hit(target, CreateAttack(parameters));

    protected virtual Attack CreateAttack(SkillParameters parameters)
        => new Attack(_type, _modifier);
}
