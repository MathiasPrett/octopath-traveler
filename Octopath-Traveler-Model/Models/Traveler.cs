using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Models;

public class Traveler : Unit
{
    private const int BaseHits = 1;
    private const double BasicAttackModifier = 1.3;
    private const double DefendingDamageReduction = 0.5;

    private readonly BoostPoints _boostPoints = new BoostPoints();
    private bool _isDefending;

    public int MaxSp { get; private set; }
    public int CurrentSp { get; private set; }
    public int Bp => _boostPoints.Current;
    public bool HasBoostPoints => _boostPoints.HasAny;
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

    public ActionReport BasicAttack(Unit target, string weaponName, int boostPoints)
    {
        _boostPoints.Spend(boostPoints);
        Attack attack = new Attack(AttackType.CreateNamed(weaponName), BasicAttackModifier);
        ActionReport report = new ActionReport(this);
        for (int hit = 0; hit < BaseHits + boostPoints; hit++)
            report.Record(Hit(target, attack));
        return report;
    }

    public ActionReport Use(ActiveSkill skill, SkillParameters parameters, int boostPoints)
    {
        CurrentSp -= skill.SpCost;
        _boostPoints.Spend(boostPoints);
        return skill.Use(parameters);
    }

    public bool CanSpendBoostPoints(int amount)
        => _boostPoints.CanSpend(amount);

    public override SpRecoveryEvent RestoreSp(int amount)
    {
        CurrentSp = Math.Min(MaxSp, CurrentSp + amount);
        return base.RestoreSp(amount);
    }

    public void Defend()
    {
        _isDefending = true;
        ClaimPriorityNextRound(TurnPriority.Defended);
    }

    public void GainBoostPoint()
        => _boostPoints.Gain();

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
