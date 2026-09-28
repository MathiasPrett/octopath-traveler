namespace Octopath_Traveler.Data;

public class ParsedTeamFile
{
    public List<ParsedTraveler> Travelers;
    public List<string> BeastNames;

    public ParsedTeamFile(List<ParsedTraveler> travelers, List<string> beastNames)
    {
        Travelers = travelers;
        BeastNames = beastNames;
    }
}
