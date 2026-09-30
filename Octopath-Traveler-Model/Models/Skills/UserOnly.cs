namespace Octopath_Traveler.Models.Skills;

public class UserOnly : SkillTargeting
{
    public override List<Unit> FindCandidates(ValidatedTeam team, Unit user)
        => new List<Unit> { user };
}
