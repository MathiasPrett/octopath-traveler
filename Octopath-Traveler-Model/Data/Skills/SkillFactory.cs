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

    // Las habilidades cuyo efecto no se deduce de Type + Modifier del JSON.
    private static readonly Dictionary<string, Func<SkillJson, Effect>> SpecialEffects = new()
    {
        [NightmareChimera] = skill => new WeaponDamageEffect(skill.Modifier),
        [LastStand] = skill => new LastStandEffect(TypeOf(skill), skill.Modifier),
        [MercyStrike] = skill => new MercyStrikeEffect(TypeOf(skill), skill.Modifier)
    };

    public static ActiveSkill Create(SkillJson skill)
    {
        string name = Require(skill.Name, "una habilidad sin nombre");
        return new ActiveSkill(name, skill.SP, TargetOf(skill), EffectsFor(name, skill));
    }

    private static List<Effect> EffectsFor(string name, SkillJson skill)
    {
        if (name == ShootingStars) return ShootingStarsEffects(skill);
        if (SpecialEffects.TryGetValue(name, out Func<SkillJson, Effect>? create)) return One(create(skill));
        return DamageEffects(skill);
    }

    private static List<Effect> ShootingStarsEffects(SkillJson skill)
        => ShootingStarsTypes
            .Select(type => (Effect)new DamageEffect(AttackType.Named(type), skill.Modifier)).ToList();

    // Sin tipo de ataque no hay daño que calcular: la habilidad se lista en el menú
    // pero todavía no tiene efecto (curaciones y habilidades de entregas siguientes).
    private static List<Effect> DamageEffects(SkillJson skill)
        => skill.Type != null && AttackType.Exists(skill.Type)
            ? One(new DamageEffect(AttackType.Named(skill.Type), skill.Modifier))
            : new List<Effect>();

    private static List<Effect> One(Effect effect)
        => new List<Effect> { effect };

    private static AttackType TypeOf(SkillJson skill)
        => AttackType.Named(Require(skill.Type, $"el tipo de {skill.Name}"));

    private static SkillTarget TargetOf(SkillJson skill)
        => Enum.Parse<SkillTarget>(Require(skill.Target, $"el objetivo de {skill.Name}"));

    private static string Require(string? value, string missing)
        => value ?? throw new InvalidDataException($"skills.json no trae {missing}");
}
