namespace Octopath_Traveler.Models.Skills;

public abstract class Skill
{
    private readonly EffectSet _effects;

    protected Skill(string name, List<Effect> effects)
    {
        Name = name;
        _effects = new EffectSet(effects);
    }

    public string Name { get; }

    protected bool NeedsWeapon => _effects.NeedsWeapon;

    protected ActionReport Apply(SkillUse use)
        => _effects.ApplyTo(use);
}
