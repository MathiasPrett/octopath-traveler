namespace Octopath_Traveler.Models.Skills;

public class ActiveSkill
{
    public string Name;
    public int SpCost;
    public string Target;

    private readonly Condition _condition;
    private readonly Effect _effect;

    public ActiveSkill(string name, int spCost, string target, Condition condition, Effect effect)
    {
        Name = name;
        SpCost = spCost;
        Target = target;
        _condition = condition;
        _effect = effect;
    }

    public bool UserMeetsUseCondition(Unit user)
        => _condition.DoesHold(user);

    public void Apply(Unit user, Unit target)
        => _effect.Apply(user, target);
}
