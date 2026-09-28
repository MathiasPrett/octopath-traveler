namespace Octopath_Traveler.Models.Skills;

public class LowestStat : TargetSelector
{
    private readonly StatType _stat;

    public LowestStat(StatType stat)
    {
        _stat = stat;
    }

    public override List<Unit> Select(List<Traveler> candidates)
        => Only(candidates.OrderBy(candidate => candidate.StatValue(_stat)).First());
}
