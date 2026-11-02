namespace Octopath_Traveler.Models.Skills;

public class SelectedOpponent : SkillTargeting
{
    private readonly TargetSelector _selector;

    public SelectedOpponent(TargetSelector selector)
    {
        _selector = selector;
    }

    public override List<Unit> FindCandidates(ValidatedTeam team, Unit user)
        => _selector.Select(team.LivingBeasts.Cast<Unit>().ToList());
}
