namespace Octopath_Traveler.Models.Skills;

public class HealEffect : Effect
{
    private readonly double _modifier;

    public HealEffect(double modifier)
    {
        _modifier = modifier;
    }

    public override void Apply(SkillParameters parameters, Unit target, ActionReport report)
        => report.Record(target.Heal(CalculateHealing(parameters.User)));

    private int CalculateHealing(Unit healer)
        => (int)Math.Floor(healer.GetDefensiveStat(AttackCategory.Elemental) * _modifier);
}
