using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class SkillFactory
{
    private const string ShootingStars = "Shooting Stars";
    private const string NightmareChimera = "Nightmare Chimera";
    private const string LastStand = "Last Stand";
    private const string MercyStrike = "Mercy Strike";

    private static readonly string[] ShootingStarsTypes = { "Wind", "Light", "Dark" };

    private static readonly Dictionary<string, SkillTargeting> Targetings = new()
    {
        ["Single"] = new ChosenTarget(new AllEnemies()),
        ["Enemies"] = new AllEnemies(),
        ["Ally"] = new ChosenTarget(new AllAllies()),
        ["Party"] = new AllAllies(),
        ["User"] = new UserOnly(),
        ["Any"] = new AllAllies()
    };

    private static readonly Dictionary<string, Func<SkillJson, List<Effect>>> SpecialEffects = new()
    {
        [ShootingStars] = CreateShootingStarsEffects,
        [NightmareChimera] = skill => AsList(new WeaponDamageEffect(skill.Modifier)),
        [LastStand] = skill => AsList(new LastStandEffect(ParseType(skill), skill.Modifier)),
        [MercyStrike] = skill => AsList(new MercyStrikeEffect(ParseType(skill), skill.Modifier))
    };

    public static ActiveSkill Create(SkillJson skill)
    {
        string name = Require(skill.Name, "una habilidad sin nombre");
        return new ActiveSkill(name, skill.SP, ParseTarget(skill), CreateEffects(name, skill));
    }

    private static List<Effect> CreateEffects(string name, SkillJson skill)
        => SpecialEffects.GetValueOrDefault(name)?.Invoke(skill) ?? CreateDamageEffects(skill);

    private static List<Effect> CreateShootingStarsEffects(SkillJson skill)
        => ShootingStarsTypes
            .Select(type => (Effect)new DamageEffect(AttackType.Named(type), skill.Modifier)).ToList();

    private static List<Effect> CreateDamageEffects(SkillJson skill)
        => DealsTypedDamage(skill)
            ? AsList(new DamageEffect(ParseType(skill), skill.Modifier))
            : new List<Effect>();

    private static bool DealsTypedDamage(SkillJson skill)
        => skill.Type != null && AttackTypeNames.Exists(skill.Type);

    private static List<Effect> AsList(Effect effect)
        => new List<Effect> { effect };

    private static AttackType ParseType(SkillJson skill)
        => AttackType.Named(Require(skill.Type, $"el tipo de {skill.Name}"));

    private static SkillTargeting ParseTarget(SkillJson skill)
        => Targetings.GetValueOrDefault(Require(skill.Target, $"el objetivo de {skill.Name}"))
           ?? throw new InvalidDataException($"{skill.Name} objetivo desconocido");

    private static string Require(string? value, string missing)
        => value ?? throw new InvalidDataException($"falta {missing}");
}
