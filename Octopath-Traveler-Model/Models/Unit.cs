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

    public int CurrentHp => _stats.HpCurrent;
    public int MaxHp => _stats.HpMax;
    public int Speed => _stats.Speed;
    public bool IsAlive => CurrentHp > 0;

    public virtual bool IsDefending => false;
    public virtual bool CanActThisRound => IsAlive;
    public virtual bool CanActNextRound => IsAlive;

    public TurnPriority PriorityThisRound { get; private set; } = TurnPriority.Normal;
    public virtual TurnPriority PriorityNextRound => _priorityNextRound;

    public int OffensiveStat(AttackCategory category) => _stats.Offensive(category);
    public int DefensiveStat(AttackCategory category) => _stats.Defensive(category);

    public void ApplyStatBonus(StatType stat, int amount) => _stats.ApplyBonus(stat, amount);

    public virtual double DamageMultiplierFor(AttackType type) => NoDamageChange;

    public virtual bool IsWeakTo(AttackType type) => false;

    public HitResult Hit(Unit target, Attack attack)
        => target.TakeHit(DamageCalculator.Calculate(this, target, attack), attack.Type);

    public virtual void EndRound()
    {
        PriorityThisRound = _priorityNextRound;
        _priorityNextRound = TurnPriority.Normal;
    }

    protected virtual HitResult TakeHit(int damage, AttackType type)
    {
        bool wasDefending = IsDefending;
        _stats.ReduceHp(damage);
        return new HitResult
        {
            Target = this,
            Damage = damage,
            Type = type,
            TargetWasDefending = wasDefending
        };
    }

    protected void ClaimPriorityNextRound(TurnPriority priority)
        => _priorityNextRound = priority;
}
