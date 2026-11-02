namespace Octopath_Traveler.Models.Skills;

public class WeaponDamageEffect : Effect
{
    private readonly double _modifier;

    public WeaponDamageEffect(double modifier)
    {
        _modifier = modifier;
    }

    public override bool NeedsWeapon => true;

    public override CombatEvent Apply(SkillParameters parameters, Unit target)
        => parameters.User.Hit(target, new Attack(parameters.Weapon, _modifier));
}
