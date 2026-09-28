using Octopath_Traveler.Models;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class BeastTurn
{
    private readonly IBattleView _battleView;
    private readonly ValidatedTeam _team;

    public BeastTurn(IBattleView battleView, ValidatedTeam team)
    {
        _battleView = battleView;
        _team = team;
    }

    public void Play(Beast beast)
        => _battleView.AnnounceBeastAttack(beast, beast.UseSkill(ChooseTarget()));

    private Traveler ChooseTarget()
        => _team.LivingTravelers()
            .OrderByDescending(traveler => traveler.CurrentHp).First();
}
