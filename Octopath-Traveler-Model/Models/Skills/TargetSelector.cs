namespace Octopath_Traveler.Models.Skills;

public abstract class TargetSelector
{
    public abstract List<Unit> Select(List<Traveler> candidates);

    protected static List<Unit> AsList(Unit target)
        => new List<Unit> { target };
}
