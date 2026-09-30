namespace Octopath_Traveler.Models.Skills;

public class PassiveSkill
{
    private readonly StatType _stat;
    private readonly int _amount;

    public PassiveSkill(StatType stat, int amount)
    {
        _stat = stat;
        _amount = amount;
    }

    public void ApplyTo(Unit carrier)
        => carrier.ApplyStatBonus(_stat, _amount);
}
