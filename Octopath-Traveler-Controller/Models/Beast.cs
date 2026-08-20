namespace Octopath_Traveler.Models;

public class Beast : Unit
{
    public string Skill;
    public int Shields;
    public List<string> Weaknesses;

    public Beast(string name, Stats stats, char position, string skill,
        int shields, List<string> weaknesses)
        : base(name, stats, position)
    {
        Skill = skill;
        Shields = shields;
        Weaknesses = weaknesses;
    }
}
