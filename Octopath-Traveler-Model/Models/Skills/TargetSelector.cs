namespace Octopath_Traveler.Models.Skills;

// A quién ataca una bestia. Los empates los resuelve el orden de tablero, porque
// OrderBy de LINQ es estable y los candidatos llegan en ese orden.
public abstract class TargetSelector
{
    public abstract List<Unit> Select(List<Traveler> candidates);

    protected static List<Unit> Only(Unit target)
        => new List<Unit> { target };
}
