namespace Octopath_Traveler.Models;

public class Traveler : Unit
{
    private const int MaxBoostPoints = 5;

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
        Bp = 0;
        Weapons = weapons;
        ActiveSkills = activeSkills;
        PassiveSkills = passiveSkills;
    }

    public void GainBoostPoint()
        => Bp = Math.Min(Bp + 1, MaxBoostPoints);
}
