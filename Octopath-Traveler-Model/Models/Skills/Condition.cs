namespace Octopath_Traveler.Models.Skills;

public abstract class Condition
{
    public abstract bool DoesHold(Unit unit);
}
