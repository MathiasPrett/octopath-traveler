using Octopath_Traveler.Models;

namespace Octopath_Traveler_View;

public interface ICombatLogView
{
    void AnnounceBasicAttack(ActionReport report);
    void AnnounceSkillUse(ActionReport report, string skillName);
    void AnnounceFlee();
}
