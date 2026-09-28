namespace Octopath_Traveler.Models.Skills;

public class HighestStat : TargetSelector
{
    private readonly StatType _stat;

    public HighestStat(StatType stat)
    {
        _stat = stat;
    }

    public override List<Unit> Select(List<Traveler> candidates)
        => Only(candidates.OrderByDescending(candidate => candidate.StatValue(_stat)).First());
}
