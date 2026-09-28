namespace Octopath_Traveler.Models;

public class AttackOutcome
{
    public Unit Attacker;
    public Unit Target;
    public int Damage;

    public AttackOutcome(Unit attacker, Unit target, int damage)
    {
        Attacker = attacker;
        Target = target;
        Damage = damage;
    }
}
