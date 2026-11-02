namespace Octopath_Traveler.Models.Skills;

public class FallenAllies : SkillTargeting
{
    public override List<Unit> FindCandidates(ValidatedTeam team, Unit user)
        => team.FallenTravelers.Cast<Unit>().ToList();
}
