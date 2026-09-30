using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Models;

public class Traveler : Unit
{
    private const int MaxBoostPoints = 5;
    private const double BasicAttackModifier = 1.3;
    private const double DefendingDamageReduction = 0.5;

    private bool _isDefending;

    public int MaxSp { get; private set; }
    public int CurrentSp { get; private set; }
    public int Bp { get; private set; }
    public IReadOnlyList<string> Weapons { get; }
    public IReadOnlyList<ActiveSkill> ActiveSkills { get; }

    public Traveler(string name, Stats stats, int maxSp,
        List<string> weapons, List<ActiveSkill> activeSkills)
        : base(name, stats)
    {
        MaxSp = maxSp;
        CurrentSp = maxSp;
        Weapons = weapons;
        ActiveSkills = activeSkills;
    }

    public override bool IsDefending => _isDefending;

    public override T Accept<T>(IUnitVisitor<T> visitor)
        => visitor.VisitTraveler(this);

    public override double GetDamageMultiplier(AttackType type)
        => _isDefending ? DefendingDamageReduction : base.GetDamageMultiplier(type);

    public IReadOnlyList<ActiveSkill> AffordableSkills
        => ActiveSkills.Where(skill => skill.SpCost <= CurrentSp).ToList();

    public ActionReport BasicAttack(Unit target, string weaponName)
    {
        ActionReport report = new ActionReport(this);
        report.Record(Hit(target, new Attack(AttackType.Named(weaponName), BasicAttackModifier)));
        return report;
    }

    public ActionReport Use(ActiveSkill skill, SkillUse use)
    {
        CurrentSp -= skill.SpCost;
        return skill.Use(use);
    }

    public void Defend()
    {
        _isDefending = true;
        ClaimPriorityNextRound(TurnPriority.Defended);
    }

    public void GainBoostPoint()
        => Bp = Math.Min(Bp + 1, MaxBoostPoints);

    public override void ApplyStatBonus(StatType stat, int amount)
    {
        if (stat == StatType.MaxSp) RaiseMaxSp(amount);
        else base.ApplyStatBonus(stat, amount);
    }

    public override void EndRound()
    {
        _isDefending = false;
        base.EndRound();
    }

    private void RaiseMaxSp(int amount)
    {
        MaxSp += amount;
        CurrentSp += amount;
    }
}
