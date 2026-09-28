namespace Octopath_Traveler.Models.Skills;

public abstract class Effect
{
    public abstract void Apply(Unit user, Unit target);
}
