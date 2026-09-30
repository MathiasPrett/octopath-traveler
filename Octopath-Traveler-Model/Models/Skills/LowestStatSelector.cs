namespace Octopath_Traveler.Models.Skills;

public class LowestStatSelector : StatTargetSelector
{
    public LowestStatSelector(StatType stat) : base(stat) { }

    public override List<Unit> Select(List<Traveler> candidates)
        => AsList(candidates.OrderBy(candidate => candidate.GetStatValue(Stat)).First());
}
