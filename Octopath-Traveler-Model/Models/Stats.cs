namespace Octopath_Traveler.Models;

public class Stats
{
    public int HpMax;
    public int HpCurrent;
    public int PhysAtk;
    public int PhysDef;
    public int ElemAtk;
    public int ElemDef;
    public int Speed;

    public Stats(int hpMax, int physAtk, int physDef, int elemAtk, int elemDef, int speed)
    {
        HpMax = hpMax;
        HpCurrent = hpMax;
        PhysAtk = physAtk;
        PhysDef = physDef;
        ElemAtk = elemAtk;
        ElemDef = elemDef;
        Speed = speed;
    }

    public void ReduceHp(int amount)
        => HpCurrent = Math.Max(0, HpCurrent - amount);

    public void ApplyBonus(StatType stat, int amount)
    {
        switch (stat)
        {
            case StatType.HpMax: RaiseMaxHp(amount); break;
            case StatType.PhysAtk: PhysAtk += amount; break;
            case StatType.PhysDef: PhysDef += amount; break;
            case StatType.ElemAtk: ElemAtk += amount; break;
            case StatType.ElemDef: ElemDef += amount; break;
            case StatType.Speed: Speed += amount; break;
        }
    }

    private void RaiseMaxHp(int amount)
    {
        HpMax += amount;
        HpCurrent += amount;
    }
}
