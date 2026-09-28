namespace Octopath_Traveler.Models;

public class AttackType
{
    private const string Unnamed = "";

    private static readonly string[] PhysicalNames =
        { "Sword", "Spear", "Dagger", "Axe", "Bow", "Stave" };

    private static readonly string[] ElementalNames =
        { "Fire", "Ice", "Lightning", "Wind", "Light", "Dark" };

    // Todas las armas del juego, en el orden en que se ofrecen al elegir una.
    public static IReadOnlyList<string> WeaponNames => PhysicalNames;

    public string Name { get; }
    public AttackCategory Category { get; }

    private AttackType(string name, AttackCategory category)
    {
        Name = name;
        Category = category;
    }

    public static AttackType Named(string name)
        => new AttackType(name, CategoryOf(name));

    public static AttackType Physical()
        => new AttackType(Unnamed, AttackCategory.Physical);

    public static AttackType Elemental()
        => new AttackType(Unnamed, AttackCategory.Elemental);

    public static bool Exists(string name)
        => PhysicalNames.Contains(name) || ElementalNames.Contains(name);

    public bool HasName => Name != Unnamed;

    private static AttackCategory CategoryOf(string name)
    {
        if (PhysicalNames.Contains(name)) return AttackCategory.Physical;
        if (ElementalNames.Contains(name)) return AttackCategory.Elemental;
        throw new InvalidDataException($"El tipo de ataque {name} no existe");
    }
}
