namespace Octopath_Traveler.Models.Skills;

public class ActiveSkill
{
    private readonly Effect _effect;

    public string Name { get; }
    public int SpCost { get; }
    public string Target { get; }

    public ActiveSkill(string name, int spCost, string target, Effect effect)
    {
        Name = name;
        SpCost = spCost;
        Target = target;
        _effect = effect;
    }

    public void Apply(Unit user, Unit target)
        => _effect.Apply(user, target);
}
