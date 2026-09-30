namespace Octopath_Traveler.Data;

public class ParsedTraveler
{
    public string Name { get; }
    public List<string> ActiveSkillNames { get; }
    public List<string> PassiveSkillNames { get; }

    public ParsedTraveler(string name, List<string> activeSkillNames, List<string> passiveSkillNames)
    {
        Name = name;
        ActiveSkillNames = activeSkillNames;
        PassiveSkillNames = passiveSkillNames;
    }
}
