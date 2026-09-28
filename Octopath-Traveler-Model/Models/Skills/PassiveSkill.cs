namespace Octopath_Traveler.Models.Skills;

// Las pasivas de esta entrega solo suben un stat base del portador.
public class PassiveSkill
{
    private readonly StatType _stat;
    private readonly int _amount;

    public PassiveSkill(string name, StatType stat, int amount)
    {
        Name = name;
        _stat = stat;
        _amount = amount;
    }

    public string Name { get; }

    public void ApplyTo(Unit carrier)
        => carrier.ApplyStatBonus(_stat, _amount);
}
