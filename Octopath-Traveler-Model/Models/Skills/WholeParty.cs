namespace Octopath_Traveler.Models.Skills;

public class WholeParty : SkillTargeting
{
    public override List<Unit> FindCandidates(ValidatedTeam team, Unit user)
        => team.LivingTravelers.OrderBy(traveler => IsUser(traveler, user)).Cast<Unit>().ToList();

    private static bool IsUser(Unit traveler, Unit user)
        => traveler == user;
}
