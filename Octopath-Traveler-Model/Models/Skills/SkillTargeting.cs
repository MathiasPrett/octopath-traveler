namespace Octopath_Traveler.Models.Skills;

public abstract class SkillTargeting
{
    public virtual bool NeedsChoice => false;

    public abstract List<Unit> FindCandidates(ValidatedTeam team, Unit user);
}
