namespace Octopath_Traveler.Models.Skills;

public class DrainHpEffect : DrainEffect
{
    public DrainHpEffect(double ratio) : base(ratio) { }

    protected override void Recover(Unit user, int amount, ActionReport report)
        => report.Record(user.Heal(amount));
}
