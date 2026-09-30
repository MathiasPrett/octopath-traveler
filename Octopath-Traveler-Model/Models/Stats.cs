namespace Octopath_Traveler.Models;

public class Stats
{
    private const int MinimumHp = 0;

    public int MaxHp { get; private set; }
    public int CurrentHp { get; private set; }
    public int PhysAtk { get; private set; }
    public int PhysDef { get; private set; }
    public int ElemAtk { get; private set; }
    public int ElemDef { get; private set; }
    public int Speed { get; private set; }

    public Stats(int maxHp, int physAtk, int physDef, int elemAtk, int elemDef, int speed)
    {
        MaxHp = maxHp;
        CurrentHp = maxHp;
        PhysAtk = physAtk;
        PhysDef = physDef;
        ElemAtk = elemAtk;
        ElemDef = elemDef;
        Speed = speed;
    }

    public int GetValue(StatType stat)
        => stat switch
        {
            StatType.MaxHp => MaxHp,
            StatType.CurrentHp => CurrentHp,
            StatType.PhysAtk => PhysAtk,
            StatType.PhysDef => PhysDef,
            StatType.ElemAtk => ElemAtk,
            StatType.ElemDef => ElemDef,
            StatType.Speed => Speed,
            _ => throw new InvalidDataException($"{stat} no existe")
        };

    public int GetOffensive(AttackCategory category)
        => category == AttackCategory.Physical ? PhysAtk : ElemAtk;

    public int GetDefensive(AttackCategory category)
        => category == AttackCategory.Physical ? PhysDef : ElemDef;

    public void ReduceHp(int amount)
        => CurrentHp = Math.Max(MinimumHp, CurrentHp - amount);

    public void ApplyBonus(StatType stat, int amount)
    {
        switch (stat)
        {
            case StatType.MaxHp: RaiseMaxHp(amount); break;
            case StatType.PhysAtk: PhysAtk += amount; break;
            case StatType.PhysDef: PhysDef += amount; break;
            case StatType.ElemAtk: ElemAtk += amount; break;
            case StatType.ElemDef: ElemDef += amount; break;
            case StatType.Speed: Speed += amount; break;
        }
    }

    private void RaiseMaxHp(int amount)
    {
        MaxHp += amount;
        CurrentHp += amount;
    }
}
