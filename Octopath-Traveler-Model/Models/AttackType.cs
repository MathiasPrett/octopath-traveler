namespace Octopath_Traveler.Models;

public class AttackType
{
    private const string Unnamed = "";

    public string Name { get; }
    public AttackCategory Category { get; }

    private AttackType(string name, AttackCategory category)
    {
        Name = name;
        Category = category;
    }

    public static AttackType Named(string name)
        => new AttackType(name, AttackTypeNames.GetCategory(name));

    public static AttackType Physical()
        => new AttackType(Unnamed, AttackCategory.Physical);

    public static AttackType Elemental()
        => new AttackType(Unnamed, AttackCategory.Elemental);

    public bool HasName => Name != Unnamed;
}
