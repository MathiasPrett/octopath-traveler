using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public interface IBattleView
{
    TravelerAction AskForAction(Traveler traveler);
    string? AskForWeapon(Traveler traveler);
    string? AskForSkill(Traveler traveler);
    Beast? AskForTarget(Traveler traveler, List<Beast> targets);
    int AskForBoostPoints();
    void AnnounceTravelerAttack(Traveler attacker, HitResult hit);
    void AnnounceBeastAttack(Beast attacker, HitResult hit);
    void AnnounceFlee();
}
