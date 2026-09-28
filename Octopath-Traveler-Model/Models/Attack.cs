namespace Octopath_Traveler.Models;

public class Attack
{
    private const double NoBonus = 1;

    public AttackType Type { get; }
    public double Modifier { get; }

    // Multiplica el daño final, después de restar la defensa (Last Stand).
    public double Bonus { get; }

    // Mercy Strike deja al objetivo en 1 HP como mínimo.
    public bool CanKill { get; }

    public Attack(AttackType type, double modifier)
        : this(type, modifier, NoBonus, true) { }

    private Attack(AttackType type, double modifier, double bonus, bool canKill)
    {
        Type = type;
        Modifier = modifier;
        Bonus = bonus;
        CanKill = canKill;
    }

    public Attack WithBonus(double bonus)
        => new Attack(Type, Modifier, bonus, CanKill);

    public Attack ThatCannotKill()
        => new Attack(Type, Modifier, Bonus, false);
}
