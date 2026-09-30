using Octopath_Traveler.Models;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class BeastTurn
{
    private readonly ICombatLogView _combatLog;
    private readonly ValidatedTeam _team;

    public BeastTurn(ICombatLogView combatLog, ValidatedTeam team)
    {
        _combatLog = combatLog;
        _team = team;
    }

    public TurnResult Play(Beast beast)
    {
        _combatLog.AnnounceSkillUse(beast.UseSkill(_team.LivingTravelers), beast.SkillName);
        return TurnResult.Completed;
    }
}
