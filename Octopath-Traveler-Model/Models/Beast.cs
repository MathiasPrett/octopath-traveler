namespace Octopath_Traveler.Models;

public class Beast : Unit
{
    // Al romperse pierde el resto de la ronda actual y toda la siguiente.
    private const int RoundsBroken = 2;
    private const int NoShields = 0;
    private const int NoDamage = 0;
    private const int NotBroken = 0;
    private const int LastRoundBroken = 1;
    private const double WeaknessOrBreakingPointMultiplier = 1.5;
    private const double WeaknessAndBreakingPointMultiplier = 2;
    private const double AttackModifier = 1.3;

    private readonly int _maxShields;
    private readonly List<string> _weaknesses;
    private int _shields;
    private int _roundsBrokenLeft;

    public string Skill { get; }

    public Beast(string name, Stats stats, string skill,
        int shields, List<string> weaknesses)
        : base(name, stats)
    {
        Skill = skill;
        _shields = shields;
        _maxShields = shields;
        _weaknesses = weaknesses;
    }

    public int Shields => _shields;
    public bool IsBroken => _roundsBrokenLeft > NotBroken;

    public override bool CanActThisRound => IsAlive && !IsBroken;
    public override bool CanActNextRound => IsAlive && _roundsBrokenLeft <= LastRoundBroken;

    public override TurnPriority PriorityNextRound
        => RecoversNextRound ? TurnPriority.RecoveringFromBreak : base.PriorityNextRound;

    public override bool IsWeakTo(AttackType type)
        => _weaknesses.Contains(type.Name);

    public override double DamageMultiplierFor(AttackType type)
    {
        if (IsWeakTo(type) && IsBroken) return WeaknessAndBreakingPointMultiplier;
        if (IsWeakTo(type) || IsBroken) return WeaknessOrBreakingPointMultiplier;
        return base.DamageMultiplierFor(type);
    }

    public HitResult UseSkill(Unit target)
        => Hit(target, new Attack(AttackType.Physical(), AttackModifier));

    public override void EndRound()
    {
        if (IsBroken) AdvanceBreakingPoint();
        base.EndRound();
    }

    protected override HitResult TakeHit(int damage, AttackType type)
    {
        bool exploitedWeakness = IsWeakTo(type);
        bool causedBreak = exploitedWeakness && LoseShield(damage);
        return base.TakeHit(damage, type) with
        {
            ExploitedWeakness = exploitedWeakness,
            CausedBreak = causedBreak
        };
    }

    private bool RecoversNextRound => _roundsBrokenLeft == LastRoundBroken;

    private bool LoseShield(int damage)
    {
        if (damage == NoDamage || IsBroken) return false;
        _shields--;
        if (_shields > NoShields) return false;
        _roundsBrokenLeft = RoundsBroken;
        return true;
    }

    private void AdvanceBreakingPoint()
    {
        _roundsBrokenLeft--;
        if (IsBroken) return;
        ClaimPriorityNextRound(TurnPriority.RecoveringFromBreak);
        if (IsAlive) _shields = _maxShields;
    }
}
