namespace Octopath_Traveler.Models.Skills;

public class EveryCandidateSelector : TargetSelector
{
    public override List<Unit> Select(List<Unit> candidates)
        => candidates.ToList();
}
