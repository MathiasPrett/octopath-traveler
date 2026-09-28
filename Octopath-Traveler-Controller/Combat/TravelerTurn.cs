using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;
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
        if (action == TravelerAction.Skill) return TryUseSkill(traveler);
        if (action == TravelerAction.Flee) return Flee();
        return Defend(traveler);
    }

    private TurnResult TryBasicAttack(Traveler traveler)
    {
        string? weapon = _battleView.AskForWeapon(traveler);
        if (weapon == null) return TurnResult.Cancelled;
        Unit? target = _battleView.AskForTarget(traveler, _team.LivingBeasts());
        if (target == null) return TurnResult.Cancelled;
        _battleView.AskForBoostPoints();
        return BasicAttack(traveler, target, weapon);
    }

    private TurnResult BasicAttack(Traveler traveler, Unit target, string weaponName)
    {
        _battleView.AnnounceBasicAttack(traveler.BasicAttack(target, weaponName));
        return TurnResult.Completed;
    }

    private TurnResult TryUseSkill(Traveler traveler)
    {
        ActiveSkill? skill = _battleView.AskForSkill(traveler);
        if (skill == null) return TurnResult.Cancelled;
        return skill.NeedsWeaponChoice
            ? TryChooseWeapon(traveler, skill)
            : ChooseTargets(traveler, skill, null);
    }

    private TurnResult TryChooseWeapon(Traveler traveler, ActiveSkill skill)
    {
        string? weapon = _battleView.AskForAnyWeapon();
        return weapon == null ? TurnResult.Cancelled : ChooseTargets(traveler, skill, weapon);
    }

    private TurnResult ChooseTargets(Traveler traveler, ActiveSkill skill, string? weaponName)
    {
        List<Unit> candidates = skill.Candidates(_team);
        if (!skill.NeedsTargetChoice) return UseSkill(traveler, skill, candidates, weaponName);
        Unit? target = _battleView.AskForTarget(traveler, candidates);
        if (target == null) return TurnResult.Cancelled;
        return UseSkill(traveler, skill, new List<Unit> { target }, weaponName);
    }

    private TurnResult UseSkill(Traveler traveler, ActiveSkill skill,
        IReadOnlyList<Unit> targets, string? weaponName)
    {
        _battleView.AskForBoostPoints();
        SkillUse use = new SkillUse(traveler, targets, weaponName);
        _battleView.AnnounceSkillUse(traveler.Use(skill, use), skill.Name);
        return TurnResult.Completed;
    }

    private TurnResult Defend(Traveler traveler)
    {
        traveler.Defend();
        return TurnResult.Completed;
    }

    private TurnResult Flee()
    {
        _battleView.AnnounceFlee();
        return TurnResult.Fled;
    }
}
