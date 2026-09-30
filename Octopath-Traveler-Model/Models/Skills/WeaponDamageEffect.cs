namespace Octopath_Traveler.Models.Skills;

public class WeaponDamageEffect : Effect
{
    private readonly double _modifier;

    public WeaponDamageEffect(double modifier)
    {
        _modifier = modifier;
    }

    public override bool NeedsWeapon => true;

    public override CombatEvent Apply(SkillUse use, Unit target)
        => use.User.Hit(target, new Attack(use.Weapon, _modifier));
}
