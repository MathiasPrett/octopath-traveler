namespace Octopath_Traveler.Models.Skills;

public class DamageEffect : Effect
{
    private const int MinimumDamage = 0;

    private readonly AttackCategory _category;
    private readonly double _modifier;

    public DamageEffect(AttackCategory category, double modifier)
    {
        _category = category;
        _modifier = modifier;
    }

    public override void Apply(Unit user, Unit target)
        => target.ReceiveDamage(CalculateDamage(user, target));

    private int CalculateDamage(Unit user, Unit target)
        => Math.Max(MinimumDamage, (int)Math.Floor(
            OffensiveStat(user) * _modifier - DefensiveStat(target)));

    private int OffensiveStat(Unit user)
        => _category == AttackCategory.Physical ? user.Stats.PhysAtk : user.Stats.ElemAtk;

    private int DefensiveStat(Unit target)
        => _category == AttackCategory.Physical ? target.Stats.PhysDef : target.Stats.ElemDef;
}
