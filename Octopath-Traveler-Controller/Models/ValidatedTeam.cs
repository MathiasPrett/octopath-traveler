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

    public List<Traveler> LivingTravelers()
        => Travelers.Where(traveler => traveler.Alive).ToList();

    public List<Beast> LivingBeasts()
        => Beasts.Where(beast => beast.Alive).ToList();

    public List<Unit> LivingUnits()
        => LivingTravelers().Cast<Unit>().Concat(LivingBeasts()).ToList();
}
