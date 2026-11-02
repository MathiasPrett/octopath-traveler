namespace Octopath_Traveler.Models.Skills;

public class WeaponDamageEffect : Effect
{
    private readonly double _modifier;

    public WeaponDamageEffect(double modifier)
    {
        _modifier = modifier;
    }

    public override bool NeedsWeapon => true;

    public override void Apply(SkillParameters parameters, Unit target, ActionReport report)
        => report.Record(parameters.User.Hit(target, new Attack(parameters.Weapon, _modifier)));
}
