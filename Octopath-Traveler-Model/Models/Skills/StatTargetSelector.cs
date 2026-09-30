namespace Octopath_Traveler.Models.Skills;

public abstract class StatTargetSelector : TargetSelector
{
    protected StatTargetSelector(StatType stat)
    {
        Stat = stat;
    }

    protected StatType Stat { get; }
}
