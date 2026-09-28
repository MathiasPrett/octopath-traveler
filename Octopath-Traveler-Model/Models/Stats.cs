namespace Octopath_Traveler.Models;

public class Stats
{
    private const int MinimumHp = 0;

    private int _physAtk;
    private int _physDef;
    private int _elemAtk;
    private int _elemDef;

    public int HpMax { get; private set; }
    public int HpCurrent { get; private set; }
    public int Speed { get; private set; }

    public Stats(int hpMax, int physAtk, int physDef, int elemAtk, int elemDef, int speed)
    {
        HpMax = hpMax;
        HpCurrent = hpMax;
        _physAtk = physAtk;
        _physDef = physDef;
        _elemAtk = elemAtk;
        _elemDef = elemDef;
        Speed = speed;
    }

    public int Offensive(AttackCategory category)
        => category == AttackCategory.Physical ? _physAtk : _elemAtk;

    public int Defensive(AttackCategory category)
        => category == AttackCategory.Physical ? _physDef : _elemDef;

    public void ReduceHp(int amount)
        => HpCurrent = Math.Max(MinimumHp, HpCurrent - amount);

    public void ApplyBonus(StatType stat, int amount)
    {
        switch (stat)
        {
            case StatType.HpMax: RaiseMaxHp(amount); break;
            case StatType.PhysAtk: _physAtk += amount; break;
            case StatType.PhysDef: _physDef += amount; break;
            case StatType.ElemAtk: _elemAtk += amount; break;
            case StatType.ElemDef: _elemDef += amount; break;
            case StatType.Speed: Speed += amount; break;
        }
    }

    private void RaiseMaxHp(int amount)
    {
        HpMax += amount;
        HpCurrent += amount;
    }
}
