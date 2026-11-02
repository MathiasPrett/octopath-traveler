namespace Octopath_Traveler.Models.Skills;

public abstract class DrainEffect : Effect
{
    private readonly double _ratio;

    protected DrainEffect(double ratio)
    {
        _ratio = ratio;
    }

    public override void Apply(SkillParameters parameters, Unit target, ActionReport report)
        => Recover(parameters.User, CalculateDrain(report.TotalDamageTo(target)), report);

    protected abstract void Recover(Unit user, int amount, ActionReport report);

    private int CalculateDrain(int damage)
        => (int)Math.Floor(damage * _ratio);
}
