namespace Octopath_Traveler.Models;

public class Beast : Unit
{
    public string Skill;
    public int Shields;
    public List<string> Weaknesses;

    public Beast(string name, Stats stats, string skill,
        int shields, List<string> weaknesses)
        : base(name, stats)
    {
        Skill = skill;
        Shields = shields;
        Weaknesses = weaknesses;
    }
}
