namespace Octopath_Traveler.Models;

public abstract class Unit
{
    public string Name;
    public Stats Stats;
    public bool Alive;

    protected Unit(string name, Stats stats)
    {
        Name = name;
        Stats = stats;
        Alive = true;
    }

    public void ReceiveDamage(int damage)
    {
        Stats.ReduceHp(damage);
        Alive = Stats.HpCurrent > 0;
    }

    public AttackOutcome Attack(Unit target)
    {
        int damage = DamageCalculator.Calculate(this, target);
        target.ReceiveDamage(damage);
        return new AttackOutcome(this, target, damage);
    }
}
