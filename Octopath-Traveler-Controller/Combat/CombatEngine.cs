using Octopath_Traveler.Models;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class CombatEngine
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
        IBattleView battleView = viewFactory.CreateBattleView();
        _travelerTurn = new TravelerTurn(battleView, team);
        _beastTurn = new BeastTurn(battleView, team);
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
        while (queue.HasPendingUnits() && !IsCombatOver())
        {
            PlayTurn(queue);
            queue.FinishCurrentTurn();
        }
    }

    private void StartRound(int round)
    {
        _roundView.AnnounceRoundStart(round);
        GrantBoostPoints();
    }

    private void GrantBoostPoints()
    {
        foreach (Traveler traveler in _team.LivingTravelers())
            traveler.GainBoostPoint();
    }

    private void PlayTurn(TurnQueue queue)
    {
        Unit actor = queue.StartCurrentTurn();
        _roundView.ShowTeamsState(_team);
        _roundView.ShowTurnQueues(queue.PendingUnits(), TurnQueue.Order(_team));
        PlayUnitTurn(actor);
    }

    private void PlayUnitTurn(Unit actor)
    {
        if (actor is Traveler traveler) PlayTravelerTurn(traveler);
        if (actor is Beast beast) _beastTurn.Play(beast);
    }

    private void PlayTravelerTurn(Traveler traveler)
    {
        if (_travelerTurn.Play(traveler) == TurnResult.Fled)
            _travelersFled = true;
    }

    private void ShowWinner()
    {
        if (AnyBeastAlive()) _roundView.AnnounceEnemyVictory();
        else _roundView.AnnouncePlayerVictory();
    }

    private bool IsCombatOver()
        => _travelersFled || !AnyTravelerAlive() || !AnyBeastAlive();

    private bool AnyTravelerAlive()
        => _team.LivingTravelers().Count > 0;

    private bool AnyBeastAlive()
        => _team.LivingBeasts().Count > 0;
}
