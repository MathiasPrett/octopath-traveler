namespace Octopath_Traveler.Models;

public static class AttackTypeNames
{
    private static readonly string[] PhysicalNames =
        { "Sword", "Spear", "Dagger", "Axe", "Bow", "Stave" };

    private static readonly string[] ElementalNames =
        { "Fire", "Ice", "Lightning", "Wind", "Light", "Dark" };

    public static IReadOnlyList<string> WeaponNames => PhysicalNames;

    public static bool Exists(string name)
        => PhysicalNames.Contains(name) || ElementalNames.Contains(name);

    public static AttackCategory GetCategory(string name)
    {
        if (PhysicalNames.Contains(name)) return AttackCategory.Physical;
        if (ElementalNames.Contains(name)) return AttackCategory.Elemental;
        throw new InvalidDataException($"{name} no existe");
    }
}
