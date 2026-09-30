namespace Octopath_Traveler.Models.Skills;

public class EveryCandidateSelector : TargetSelector
{
    public override List<Unit> Select(List<Traveler> candidates)
        => candidates.Cast<Unit>().ToList();
}
