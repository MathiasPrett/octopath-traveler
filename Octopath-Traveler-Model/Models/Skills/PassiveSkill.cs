namespace Octopath_Traveler.Models.Skills;

public class PassiveSkill
{
    public string Name;
    public string Target;

    private readonly Effect _effect;

    public PassiveSkill(string name, string target, Effect effect)
    {
        Name = name;
        Target = target;
        _effect = effect;
    }

    public void Apply(Unit carrier)
        => _effect.Apply(carrier, carrier);
}
