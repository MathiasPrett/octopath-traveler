namespace Octopath_Traveler.Models;

public class Traveler : Unit
{
    public int SpMax;
    public int SpCurrent;
    public int Bp;
    public List<string> Weapons;
    public List<string> ActiveSkills;
    public List<string> PassiveSkills;

    public Traveler(string name, Stats stats, int spMax,
        List<string> weapons, List<string> activeSkills, List<string> passiveSkills)
        : base(name, stats)
    {
        SpMax = spMax;
        SpCurrent = spMax;
        Bp = 1;
        Weapons = weapons;
        ActiveSkills = activeSkills;
        PassiveSkills = passiveSkills;
    }
}
