namespace Octopath_Traveler.Models;

// El orden de los valores es el orden en que las unidades juegan la ronda.
public enum TurnPriority
{
    RecoveringFromBreak,
    Defended,
    Spearheaded,
    Normal,
    Trapped
}
