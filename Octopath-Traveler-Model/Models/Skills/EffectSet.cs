namespace Octopath_Traveler.Models.Skills;

public class EffectSet
{
    private readonly List<Effect> _effects;

    public EffectSet(List<Effect> effects)
    {
        _effects = effects;
    }

    public bool NeedsWeapon
        => _effects.Any(effect => effect.NeedsWeapon);

    public void ApplyTo(SkillParameters parameters, ActionReport report)
    {
        foreach (Unit target in parameters.Targets)
            ApplyAll(parameters, target, report);
    }

    private void ApplyAll(SkillParameters parameters, Unit target, ActionReport report)
    {
        foreach (Effect effect in _effects)
            effect.Apply(parameters, target, report);
    }
}
