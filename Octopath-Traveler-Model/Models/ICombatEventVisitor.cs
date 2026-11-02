namespace Octopath_Traveler.Models;

public interface ICombatEventVisitor
{
    void Visit(DamageEvent damage);
    void Visit(HealEvent heal);
    void Visit(ReviveEvent revive);
    void Visit(TrapEvent trap);
    void Visit(SpRecoveryEvent spRecovery);
}
