namespace Octopath_Traveler.Models;

public abstract class Unit
{
    public string Name;
    public Stats Stats;
    public char Position;
    public bool Alive;

    protected Unit(string name, Stats stats, char position)
    {
        Name = name;
        Stats = stats;
        Position = position;
        Alive = true;
    }
}
