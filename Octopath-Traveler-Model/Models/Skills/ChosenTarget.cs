namespace Octopath_Traveler.Models.Skills;

public class ChosenTarget : SkillTargeting
{
    private readonly SkillTargeting _pool;

    public ChosenTarget(SkillTargeting pool)
    {
        _pool = pool;
    }

    public override bool NeedsChoice => true;

    public override List<Unit> FindCandidates(ValidatedTeam team, Unit user)
        => _pool.FindCandidates(team, user);
}
