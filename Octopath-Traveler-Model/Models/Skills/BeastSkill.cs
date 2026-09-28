namespace Octopath_Traveler.Models.Skills;

public class BeastSkill
{
    private readonly TargetSelector _selector;
    private readonly EffectSet _effects;

    public BeastSkill(string name, TargetSelector selector, EffectSet effects)
    {
        Name = name;
        _selector = selector;
        _effects = effects;
    }

    public string Name { get; }

    public ActionReport Use(Unit user, List<Traveler> candidates)
        => _effects.ApplyTo(new SkillUse(user, _selector.Select(candidates)));
}
