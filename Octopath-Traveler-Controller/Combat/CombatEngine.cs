using Octopath_Traveler.Models;
using Octopath_Traveler_View;

namespace Octopath_Traveler.Combat;

public class CombatEngine
{
    private const int FirstRound = 1;

    private readonly ValidatedTeam _team;
    private readonly CombatRenderer _renderer;
    private readonly TravelerTurn _travelerTurn;
    private readonly BeastTurn _beastTurn;
    private bool _travelersFled;

    public CombatEngine(View view, ValidatedTeam team)
    {
        _team = team;
        _renderer = new CombatRenderer(view);
        _travelerTurn = new TravelerTurn(view, _renderer, team);
        _beastTurn = new BeastTurn(_renderer, team);
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
        _renderer.ShowRoundStart(round);
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
        _renderer.ShowTeamsState(_team);
        _renderer.ShowTurnQueues(queue.PendingUnits(), TurnQueue.Order(_team));
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
        if (AnyBeastAlive()) _renderer.ShowEnemyVictory();
        else _renderer.ShowPlayerVictory();
    }

    private bool IsCombatOver()
        => _travelersFled || !AnyTravelerAlive() || !AnyBeastAlive();

    private bool AnyTravelerAlive()
        => _team.LivingTravelers().Count > 0;

    private bool AnyBeastAlive()
        => _team.LivingBeasts().Count > 0;
}
