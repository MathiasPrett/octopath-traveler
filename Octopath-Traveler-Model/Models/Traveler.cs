namespace Octopath_Traveler.Models;

public class Traveler : Unit
{
    private const int MaxBoostPoints = 5;
    private const double BasicAttackModifier = 1.3;
    private const double DefendingDamageReduction = 0.5;

    private bool _isDefending;

    public int SpMax { get; private set; }
    public int SpCurrent { get; private set; }
    public int Bp { get; private set; }
    public IReadOnlyList<string> Weapons { get; }
    public IReadOnlyList<string> ActiveSkills { get; }
    public IReadOnlyList<string> PassiveSkills { get; }

    public Traveler(string name, Stats stats, int spMax,
        List<string> weapons, List<string> activeSkills, List<string> passiveSkills)
        : base(name, stats)
    {
        SpMax = spMax;
        SpCurrent = spMax;
        Weapons = weapons;
        ActiveSkills = activeSkills;
        PassiveSkills = passiveSkills;
    }

    public override bool IsDefending => _isDefending;

    public override double DamageMultiplierFor(AttackType type)
        => _isDefending ? DefendingDamageReduction : base.DamageMultiplierFor(type);

    public HitResult BasicAttack(Unit target, string weaponName)
        => Hit(target, new Attack(AttackType.Named(weaponName), BasicAttackModifier));

    public void Defend()
    {
        _isDefending = true;
        ClaimPriorityNextRound(TurnPriority.Defended);
    }

    public void GainBoostPoint()
        => Bp = Math.Min(Bp + 1, MaxBoostPoints);

    public override void EndRound()
    {
        _isDefending = false;
        base.EndRound();
    }
}
