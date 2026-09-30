namespace Octopath_Traveler.Models;

public interface ICombatEventVisitor
{
    void Visit(DamageEvent damage);
}
