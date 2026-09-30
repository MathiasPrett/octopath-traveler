namespace Octopath_Traveler.Models.Skills;

public class AllEnemies : SkillTargeting
{
    public override List<Unit> FindCandidates(ValidatedTeam team, Unit user)
        => team.LivingBeasts.Cast<Unit>().ToList();
}
