namespace Octopath_Traveler.Models;

public class BreakingPoint
{
    private const int RoundsBroken = 2;
    private const int NoShields = 0;
    private const int NoDamage = 0;
    private const int NotBroken = 0;
    private const int LastRoundBroken = 1;

    private readonly int _maxShields;
    private int _roundsBrokenLeft;

    public BreakingPoint(int shields)
    {
        Shields = shields;
        _maxShields = shields;
    }

    public int Shields { get; private set; }
    public bool IsBroken => _roundsBrokenLeft > NotBroken;
    public bool IsBrokenNextRound => _roundsBrokenLeft > LastRoundBroken;
    public bool RecoversNextRound => _roundsBrokenLeft == LastRoundBroken;

    public void LoseShield(int damage)
    {
        if (!CanLoseShield(damage)) return;
        Shields--;
        if (Shields <= NoShields) _roundsBrokenLeft = RoundsBroken;
    }

    public void AdvanceRound()
        => _roundsBrokenLeft--;

    public void RestoreShields()
        => Shields = _maxShields;

    private bool CanLoseShield(int damage)
        => damage != NoDamage && !IsBroken;
}
