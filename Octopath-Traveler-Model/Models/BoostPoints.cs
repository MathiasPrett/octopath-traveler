namespace Octopath_Traveler.Models;

public class BoostPoints
{
    private const int Max = 5;
    private const int None = 0;

    private bool _skipNextGain;

    public int Current { get; private set; } = None;

    public bool HasAny => Current > None;

    public bool CanSpend(int amount)
        => amount <= Current;

    public void Gain()
    {
        if (_skipNextGain)
            _skipNextGain = false;
        else
            Current = Math.Min(Current + 1, Max);
    }

    public void Spend(int amount)
    {
        if (amount == None) return;
        Current -= amount;
        _skipNextGain = true;
    }
}
