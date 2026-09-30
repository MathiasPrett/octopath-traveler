namespace Octopath_Traveler.Models;

public abstract class Unit
{
    protected const double NoDamageChange = 1;

    private readonly Stats _stats;
    private TurnPriority _priorityNextRound = TurnPriority.Normal;

    public string Name { get; }

    protected Unit(string name, Stats stats)
    {
        Name = name;
        _stats = stats;
    }

    public int CurrentHp => _stats.CurrentHp;
    public int MaxHp => _stats.MaxHp;
    public int Speed => _stats.Speed;
    public bool IsAlive => CurrentHp > 0;

    public virtual bool IsDefending => false;
    public virtual bool CanActThisRound => IsAlive;
    public virtual bool CanActNextRound => IsAlive;

    public TurnPriority PriorityThisRound { get; private set; } = TurnPriority.Normal;
    public virtual TurnPriority PriorityNextRound => _priorityNextRound;

    public int GetOffensiveStat(AttackCategory category) => _stats.GetOffensive(category);
    public int GetDefensiveStat(AttackCategory category) => _stats.GetDefensive(category);

    public int GetStatValue(StatType stat) => _stats.GetValue(stat);

    public virtual void ApplyStatBonus(StatType stat, int amount) => _stats.ApplyBonus(stat, amount);

    public virtual double GetDamageMultiplier(AttackType type) => NoDamageChange;

    public abstract T Accept<T>(IUnitVisitor<T> visitor);

    public DamageEvent Hit(Unit target, Attack attack)
        => target.TakeHit(DamageCalculator.Calculate(this, target, attack), attack.Type);

    public DamageEvent TakeDirectDamage(int damage)
    {
        ReduceHp(damage);
        return new DamageEvent { Target = this, Damage = damage };
    }

    public virtual void EndRound()
    {
        PriorityThisRound = _priorityNextRound;
        _priorityNextRound = TurnPriority.Normal;
    }

    protected virtual DamageEvent TakeHit(int damage, AttackType? type)
    {
        bool wasDefending = IsDefending;
        ReduceHp(damage);
        return new DamageEvent
        {
            Target = this,
            Damage = damage,
            Type = type,
            TargetWasDefending = wasDefending
        };
    }

    protected void ReduceHp(int damage)
        => _stats.ReduceHp(damage);

    protected void ClaimPriorityNextRound(TurnPriority priority)
        => _priorityNextRound = priority;
}
