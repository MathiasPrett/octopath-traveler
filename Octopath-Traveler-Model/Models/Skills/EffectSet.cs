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

    public ActionReport ApplyTo(SkillParameters parameters)
    {
        ActionReport report = new ActionReport(parameters.User);
        foreach (CombatEvent combatEvent in parameters.Targets.SelectMany(target => ApplyAll(parameters, target)))
            report.Record(combatEvent);
        return report;
    }

    private IEnumerable<CombatEvent> ApplyAll(SkillParameters parameters, Unit target)
        => _effects.Select(effect => effect.Apply(parameters, target));
}
