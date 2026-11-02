namespace Octopath_Traveler.Models.Skills;

public class ClaimPriorityEffect : Effect
{
    private readonly TurnPriority _priority;

    public ClaimPriorityEffect(TurnPriority priority)
    {
        _priority = priority;
    }

    public override void Apply(SkillParameters parameters, Unit target, ActionReport report)
        => parameters.User.ClaimPriorityNextRound(_priority);
}
