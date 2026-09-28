using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class AttackTypeCatalog
{
    private static readonly HashSet<string> PhysicalTypes =
        new() { "Sword", "Spear", "Axe", "Dagger", "Bow", "Stave" };

    private static readonly HashSet<string> ElementalTypes =
        new() { "Fire", "Ice", "Lightning", "Wind", "Light", "Dark" };

    public static AttackCategory? Classify(string? type)
    {
        if (type != null && PhysicalTypes.Contains(type)) return AttackCategory.Physical;
        if (type != null && ElementalTypes.Contains(type)) return AttackCategory.Elemental;
        return null;
    }
}
