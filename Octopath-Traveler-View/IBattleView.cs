using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler_View;

public interface IBattleView
{
    TravelerAction AskForAction(Traveler traveler);
    string? AskForWeapon(Traveler traveler);
    string? AskForAnyWeapon();
    ActiveSkill? AskForSkill(Traveler traveler);
    Unit? AskForTarget(Traveler traveler, IReadOnlyList<Unit> targets);
    int AskForBoostPoints();
    void AnnounceBasicAttack(ActionReport report);
    void AnnounceSkillUse(ActionReport report, string skillName);
    void AnnounceFlee();
}
