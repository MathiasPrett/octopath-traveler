namespace Octopath_Traveler.Models.Skills;

public class HalveHpEffect : Effect
{
    private const int Half = 2;

    public override void Apply(SkillParameters parameters, Unit target, ActionReport report)
        => report.Record(target.TakeDirectDamage(target.CurrentHp - target.CurrentHp / Half));
}
