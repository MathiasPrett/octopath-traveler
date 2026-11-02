namespace Octopath_Traveler.Models.Skills;

public class ReviveEffect : Effect
{
    public override void Apply(SkillParameters parameters, Unit target, ActionReport report)
        => report.Record(target.Revive());
}
