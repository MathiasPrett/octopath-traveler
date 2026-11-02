namespace Octopath_Traveler.Models.Skills;

public class TrapEffect : Effect
{
    private readonly int _rounds;

    public TrapEffect(int rounds)
    {
        _rounds = rounds;
    }

    public override void Apply(SkillParameters parameters, Unit target, ActionReport report)
        => report.Record(target.Trap(_rounds));
}
