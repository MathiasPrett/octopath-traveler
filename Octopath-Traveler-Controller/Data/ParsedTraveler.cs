namespace Octopath_Traveler.Data;

public class ParsedTraveler
{
    public string Name;
    public List<string> ActiveSkillNames;
    public List<string> PassiveSkillNames;

    public ParsedTraveler(string name, List<string> activeSkillNames, List<string> passiveSkillNames)
    {
        Name = name;
        ActiveSkillNames = activeSkillNames;
        PassiveSkillNames = passiveSkillNames;
    }
}
