namespace Octopath_Traveler.Data;

public class ParsedTeamFile
{
    public List<ParsedTraveler> Travelers { get; }
    public List<string> BeastNames { get; }

    public ParsedTeamFile(List<ParsedTraveler> travelers, List<string> beastNames)
    {
        Travelers = travelers;
        BeastNames = beastNames;
    }
}
