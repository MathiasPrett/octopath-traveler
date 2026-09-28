namespace Octopath_Traveler.Models;

public class Attack
{
    public AttackType Type { get; }
    public double Modifier { get; }

    public Attack(AttackType type, double modifier)
    {
        Type = type;
        Modifier = modifier;
    }
}
