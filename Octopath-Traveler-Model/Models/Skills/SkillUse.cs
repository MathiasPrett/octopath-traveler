namespace Octopath_Traveler.Models.Skills;

// Los datos de un uso concreto de una habilidad: quién la usa, sobre quiénes y con
// qué arma, cuando la habilidad pide elegir una.
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
        => _weapon ?? throw new InvalidDataException("La habilidad se usó sin elegir un arma");
}
