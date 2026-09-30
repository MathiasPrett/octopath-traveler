namespace Octopath_Traveler.Models.Skills;

public class SkillUse
{
    private readonly AttackType? _weapon;

    public SkillUse(Unit user, IReadOnlyList<Unit> targets)
        : this(user, targets, null) { }

    public SkillUse(Unit user, IReadOnlyList<Unit> targets, string? weaponName)
    {
        User = user;
        Targets = targets;
        _weapon = weaponName == null ? null : AttackType.Named(weaponName);
    }

    public Unit User { get; }
    public IReadOnlyList<Unit> Targets { get; }

    public AttackType Weapon
        => _weapon ?? throw new InvalidDataException("sin arma");
}
