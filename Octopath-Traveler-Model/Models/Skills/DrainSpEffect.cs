namespace Octopath_Traveler.Models.Skills;

public class DrainSpEffect : DrainEffect
{
    private const int NothingDrained = 0;

    public DrainSpEffect(double ratio) : base(ratio) { }

    protected override void Recover(Unit user, int amount, ActionReport report)
    {
        if (amount == NothingDrained) return;
        report.Record(user.RestoreSp(amount));
    }
}
