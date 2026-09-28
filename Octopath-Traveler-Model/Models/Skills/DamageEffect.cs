namespace Octopath_Traveler.Models.Skills;

public class DamageEffect : Effect
{
    private readonly Attack _attack;

    public DamageEffect(AttackType type, double modifier)
    {
        _attack = new Attack(type, modifier);
    }

    public override void Apply(Unit user, Unit target)
        => user.Hit(target, _attack);
}
