namespace Octopath_Traveler.Models.Skills;

// Los efectos de una habilidad, aplicados objetivo por objetivo. Ese orden es el que
// siguen los mensajes de las habilidades que golpean varias veces o a varias unidades.
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
        foreach (Unit target in use.Targets)
            ApplyAll(report, use, target);
        return report;
    }

    private void ApplyAll(ActionReport report, SkillUse use, Unit target)
    {
        foreach (Effect effect in _effects)
            report.Record(effect.Apply(use, target));
    }
}
