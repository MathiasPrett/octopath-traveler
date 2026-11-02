namespace Octopath_Traveler.Models.Skills;

public class HalveHpEffect : Effect
{
    private const int Half = 2;

    public override CombatEvent Apply(SkillParameters parameters, Unit target)
        => target.TakeDirectDamage(target.CurrentHp - target.CurrentHp / Half);
}
