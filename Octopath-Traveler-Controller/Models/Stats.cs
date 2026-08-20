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
}
