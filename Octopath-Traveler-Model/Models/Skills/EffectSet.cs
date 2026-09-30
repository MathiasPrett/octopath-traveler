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

    public ActionReport ApplyTo(SkillUse use)
    {
        ActionReport report = new ActionReport(use.User);
        foreach (CombatEvent combatEvent in use.Targets.SelectMany(target => ApplyAll(use, target)))
            report.Record(combatEvent);
        return report;
    }

    private IEnumerable<CombatEvent> ApplyAll(SkillUse use, Unit target)
        => _effects.Select(effect => effect.Apply(use, target));
}
