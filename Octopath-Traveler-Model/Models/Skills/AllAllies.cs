namespace Octopath_Traveler.Models.Skills;

public class AllAllies : SkillTargeting
{
    public override List<Unit> FindCandidates(ValidatedTeam team, Unit user)
        => team.LivingTravelers.Cast<Unit>().ToList();
}
