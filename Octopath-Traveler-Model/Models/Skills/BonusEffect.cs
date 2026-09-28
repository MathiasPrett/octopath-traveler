namespace Octopath_Traveler.Models.Skills;

public class BonusEffect : Effect
{
    private readonly StatType _stat;
    private readonly int _amount;

    public BonusEffect(StatType stat, int amount)
    {
        _stat = stat;
        _amount = amount;
    }

    public override void Apply(Unit user, Unit target)
        => target.Stats.ApplyBonus(_stat, _amount);
}
