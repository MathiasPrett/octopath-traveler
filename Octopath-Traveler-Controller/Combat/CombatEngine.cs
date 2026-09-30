using Octopath_Traveler.Models;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class CombatEngine : IUnitVisitor<TurnResult>
{
    private const int FirstRound = 1;

    private readonly ValidatedTeam _team;
    private readonly IRoundView _roundView;
    private readonly TravelerTurn _travelerTurn;
    private readonly BeastTurn _beastTurn;
    private bool _travelersFled;

    public CombatEngine(IViewFactory viewFactory, ValidatedTeam team)
    {
        _team = team;
        _roundView = viewFactory.CreateRoundView();
        ICombatLogView combatLog = viewFactory.CreateCombatLogView();
        _travelerTurn = new TravelerTurn(viewFactory.CreateBattleView(), combatLog, team);
        _beastTurn = new BeastTurn(combatLog, team);
    }

    public void Run()
    {
        int round = FirstRound;
        while (!IsCombatOver())
        {
            PlayRound(round);
            round++;
        }
        ShowWinner();
    }

    private void PlayRound(int round)
    {
        StartRound(round);
        TurnQueue queue = new TurnQueue(_team);
        while (ShouldContinueRound(queue))
            PlayTurn(queue);
        EndRound();
    }

    private void StartRound(int round)
    {
        _roundView.AnnounceRoundStart(round);
        GrantBoostPoints();
    }

    private void GrantBoostPoints()
    {
        foreach (Traveler traveler in _team.LivingTravelers)
            traveler.GainBoostPoint();
    }

    private void EndRound()
    {
        foreach (Unit unit in _team.AllUnits)
            unit.EndRound();
    }

    private void PlayTurn(TurnQueue queue)
    {
        Unit actor = queue.NextUnit;
        _roundView.ShowTeamsState(_team);
        _roundView.ShowTurnQueues(queue.PendingUnits, TurnQueue.GetNextRoundOrder(_team));
        PlayUnitTurn(actor);
        queue.MarkPlayed(actor);
    }

    private void PlayUnitTurn(Unit actor)
    {
        if (actor.Accept(this) == TurnResult.Fled)
            _travelersFled = true;
    }

    TurnResult IUnitVisitor<TurnResult>.VisitTraveler(Traveler traveler)
        => _travelerTurn.Play(traveler);

    TurnResult IUnitVisitor<TurnResult>.VisitBeast(Beast beast)
        => _beastTurn.Play(beast);

    private void ShowWinner()
    {
        if (IsAnyBeastAlive()) _roundView.AnnounceEnemyVictory();
        else _roundView.AnnouncePlayerVictory();
    }

    private bool ShouldContinueRound(TurnQueue queue)
        => queue.HasPendingUnits() && !IsCombatOver();

    private bool IsCombatOver()
        => _travelersFled || !IsAnyTravelerAlive() || !IsAnyBeastAlive();

    private bool IsAnyTravelerAlive()
        => _team.LivingTravelers.Count > 0;

    private bool IsAnyBeastAlive()
        => _team.LivingBeasts.Count > 0;
}
