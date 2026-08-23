namespace Octopath_Traveler.Models;

public class ValidatedTeam
{
    public List<Traveler> Travelers;
    public List<Beast> Beasts;

    public ValidatedTeam(List<Traveler> travelers, List<Beast> beasts)
    {
        Travelers = travelers;
        Beasts = beasts;
    }
}
