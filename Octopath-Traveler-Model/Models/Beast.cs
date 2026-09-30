using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Models;

public class Beast : Unit
{
    private const double WeaknessOrBreakingPointMultiplier = 1.5;
    private const double WeaknessAndBreakingPointMultiplier = 2;

    private readonly List<string> _weaknesses;
    private readonly BeastSkill _skill;
    private readonly BreakingPoint _breakingPoint;

    public Beast(string name, Stats stats, BeastSkill skill,
        int shields, List<string> weaknesses)
        : base(name, stats)
    {
        _skill = skill;
        _breakingPoint = new BreakingPoint(shields);
        _weaknesses = weaknesses;
    }

    public int Shields => _breakingPoint.Shields;
    public string SkillName => _skill.Name;

    public override bool CanActThisRound => IsAlive && !IsBroken;
    public override bool CanActNextRound => IsAlive && !_breakingPoint.IsBrokenNextRound;

    public override T Accept<T>(IUnitVisitor<T> visitor)
        => visitor.VisitBeast(this);

    public override TurnPriority PriorityNextRound
        => RecoversNextRound ? TurnPriority.RecoveringFromBreak : base.PriorityNextRound;

    public override double GetDamageMultiplier(AttackType type)
    {
        if (IsWeakAndBroken(type)) return WeaknessAndBreakingPointMultiplier;
        if (IsWeakOrBroken(type)) return WeaknessOrBreakingPointMultiplier;
        return base.GetDamageMultiplier(type);
    }

    public ActionReport UseSkill(List<Traveler> travelers)
        => _skill.Use(this, travelers);

    public override void EndRound()
    {
        if (IsBroken) AdvanceBreakingPoint();
        base.EndRound();
    }

    protected override DamageEvent TakeHit(int damage, AttackType? type)
    {
        bool exploitedWeakness = type != null && IsWeakTo(type);
        bool wasBroken = IsBroken;
        if (exploitedWeakness) _breakingPoint.LoseShield(damage);
        return base.TakeHit(damage, type) with
        {
            ExploitedWeakness = exploitedWeakness,
            CausedBreak = !wasBroken && IsBroken
        };
    }

    private bool IsBroken => _breakingPoint.IsBroken;
    private bool RecoversNextRound => _breakingPoint.RecoversNextRound;

    private bool IsWeakTo(AttackType type)
        => _weaknesses.Contains(type.Name);

    private bool IsWeakAndBroken(AttackType type)
        => IsWeakTo(type) && IsBroken;

    private bool IsWeakOrBroken(AttackType type)
        => IsWeakTo(type) || IsBroken;

    private void AdvanceBreakingPoint()
    {
        _breakingPoint.AdvanceRound();
        if (IsBroken) return;
        ClaimPriorityNextRound(TurnPriority.RecoveringFromBreak);
        if (IsAlive) _breakingPoint.RestoreShields();
    }
}
