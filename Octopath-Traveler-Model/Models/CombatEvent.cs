namespace Octopath_Traveler.Models;

// Lo que le pasó a una unidad durante una acción, en datos: la vista es la que
// decide cómo se redacta.
public abstract record CombatEvent
{
    public required Unit Target { get; init; }

    // Los eventos que no tocan el HP no llevan línea de "termina con HP".
    public virtual bool ChangedHp => true;
}
