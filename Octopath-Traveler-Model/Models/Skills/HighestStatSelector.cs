namespace Octopath_Traveler.Models.Skills;

public class HighestStatSelector : StatTargetSelector
{
    public HighestStatSelector(StatType stat) : base(stat) { }

    public override List<Unit> Select(List<Unit> candidates)
        => AsList(candidates.OrderByDescending(candidate => candidate.GetStatValue(Stat)).First());
}
