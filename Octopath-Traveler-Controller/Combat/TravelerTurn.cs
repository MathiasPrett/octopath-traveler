using Octopath_Traveler.Models;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class TravelerTurn
{
    private readonly IBattleView _battleView;
    private readonly ValidatedTeam _team;

    public TravelerTurn(IBattleView battleView, ValidatedTeam team)
    {
        _battleView = battleView;
        _team = team;
    }

    public TurnResult Play(Traveler traveler)
    {
        TurnResult result = TurnResult.Cancelled;
        while (result == TurnResult.Cancelled)
            result = ChooseAndExecuteAction(traveler);
        return result;
    }

    private TurnResult ChooseAndExecuteAction(Traveler traveler)
        => ExecuteAction(traveler, _battleView.AskForAction(traveler));

    private TurnResult ExecuteAction(Traveler traveler, TravelerAction action)
    {
        if (action == TravelerAction.BasicAttack) return TryBasicAttack(traveler);
        if (action == TravelerAction.Skill) return BrowseSkills(traveler);
        if (action == TravelerAction.Flee) return Flee();
        return TurnResult.Completed;
    }

    private TurnResult TryBasicAttack(Traveler traveler)
    {
        string? weapon = _battleView.AskForWeapon(traveler);
        if (weapon == null) return TurnResult.Cancelled;
        Beast? target = _battleView.AskForTarget(traveler, _team.LivingBeasts());
        if (target == null) return TurnResult.Cancelled;
        _battleView.AskForBoostPoints();
        return Attack(traveler, target, weapon);
    }

    private TurnResult Attack(Traveler attacker, Beast target, string weaponName)
    {
        _battleView.AnnounceTravelerAttack(attacker.Attack(target), weaponName);
        return TurnResult.Completed;
    }

    private TurnResult BrowseSkills(Traveler traveler)
    {
        _battleView.AskForSkill(traveler);
        return TurnResult.Cancelled;
    }

    private TurnResult Flee()
    {
        _battleView.AnnounceFlee();
        return TurnResult.Fled;
    }
}
