namespace Octopath_Traveler.Models.Skills;

public class EveryCandidate : TargetSelector
{
    public override List<Unit> Select(List<Traveler> candidates)
        => candidates.Cast<Unit>().ToList();
}
