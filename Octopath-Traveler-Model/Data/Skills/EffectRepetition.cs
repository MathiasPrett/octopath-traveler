using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class EffectRepetition
{
    public static List<Effect> Repeat(Effect effect, int hits)
        => Enumerable.Repeat(effect, hits).ToList();
}
