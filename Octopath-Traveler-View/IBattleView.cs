using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public interface IBattleView
{
    TravelerAction AskForAction(Traveler traveler);
    string? AskForWeapon(Traveler traveler);
    string? AskForSkill(Traveler traveler);
    Beast? AskForTarget(Traveler traveler, List<Beast> targets);
    int AskForBoostPoints();
    void AnnounceTravelerAttack(AttackOutcome outcome, string weaponName);
    void AnnounceBeastAttack(AttackOutcome outcome, string skillName);
    void AnnounceFlee();
}
