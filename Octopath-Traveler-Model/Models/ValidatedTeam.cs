namespace Octopath_Traveler.Models;

public class ValidatedTeam
{
    private readonly List<Traveler> _travelers;
    private readonly List<Beast> _beasts;

    public ValidatedTeam(List<Traveler> travelers, List<Beast> beasts)
    {
        _travelers = travelers;
        _beasts = beasts;
    }

    public IReadOnlyList<Traveler> Travelers => _travelers;
    public IReadOnlyList<Beast> Beasts => _beasts;

    public List<Traveler> LivingTravelers()
        => _travelers.Where(traveler => traveler.IsAlive).ToList();

    public List<Beast> LivingBeasts()
        => _beasts.Where(beast => beast.IsAlive).ToList();

    public List<Unit> LivingUnits()
        => LivingTravelers().Cast<Unit>().Concat(LivingBeasts()).ToList();

    public List<Unit> AllUnits()
        => _travelers.Cast<Unit>().Concat(_beasts).ToList();
}
