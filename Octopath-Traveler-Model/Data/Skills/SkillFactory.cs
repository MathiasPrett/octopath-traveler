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
    private const string HealWounds = "Heal Wounds";
    private const string HealMore = "Heal More";
    private const string FirstAid = "First Aid";
    private const string Vivify = "Vivify";
    private const string Revive = "Revive";
    private const string LegholdTrap = "Leghold Trap";
    private const string Spearhead = "Spearhead";
    private const string HpThief = "HP Thief";
    private const string StealSp = "Steal SP";
    private const string ThousandSpears = "Thousand Spears";
    private const string RainOfArrows = "Rain of Arrows";
    private const string GuardianLiondog = "Guardian Liondog";

    private const int LegholdTrapRounds = 2;
    private const int SingleHit = 1;
    private const double HpThiefRatio = 0.5;
    private const double StealSpRatio = 0.05;

    private static readonly string[] ShootingStarsTypes = { "Wind", "Light", "Dark" };

    private static readonly Dictionary<string, int> HitsBySkill = new()
    {
        ["Fire Storm"] = 2, ["Blizzard"] = 2, ["Lightning Blast"] = 2,
        [HpThief] = 2, [StealSp] = 2,
        ["Ignis Ardere"] = 3, ["Glacies Claudere"] = 3, ["Tonitrus Canere"] = 3,
        ["Ventus Saltare"] = 3, ["Lux Congerere"] = 3, ["Tenebrae Operire"] = 3,
        [GuardianLiondog] = 5,
        [RainOfArrows] = 6, ["Arrowstorm"] = 6,
        [ThousandSpears] = 7
    };

    private static readonly Dictionary<string, SkillTargeting> Targetings = new()
    {
        ["Single"] = new ChosenTarget(new AllOpponents()),
        ["Enemies"] = new AllOpponents(),
        ["Ally"] = new ChosenTarget(new AllAllies()),
        ["Party"] = new WholeParty(),
        ["User"] = new UserOnly(),
        ["Any"] = new AllAllies()
    };

    private static readonly Dictionary<string, Func<SkillJson, List<Effect>>> SpecialEffects = new()
    {
        [ShootingStars] = CreateShootingStarsEffects,
        [NightmareChimera] = skill => AsList(new WeaponDamageEffect(skill.Modifier)),
        [LastStand] = skill => AsList(new LastStandEffect(ParseType(skill), skill.Modifier)),
        [MercyStrike] = skill => AsList(new MercyStrikeEffect(ParseType(skill), skill.Modifier)),
        [HealWounds] = CreateHealEffects,
        [HealMore] = CreateHealEffects,
        [FirstAid] = CreateHealEffects,
        [Vivify] = skill => new List<Effect> { new ReviveEffect(), new HealEffect(skill.Modifier) },
        [Revive] = _ => AsList(new ReviveEffect()),
        [LegholdTrap] = _ => AsList(new TrapEffect(LegholdTrapRounds)),
        [Spearhead] = skill => new List<Effect>
            { CreateTypedDamage(skill), new ClaimPriorityEffect(TurnPriority.Spearheaded) },
        [HpThief] = skill => CreateDrainingAttack(skill, new DrainHpEffect(HpThiefRatio)),
        [StealSp] = skill => CreateDrainingAttack(skill, new DrainSpEffect(StealSpRatio))
    };

    private static readonly Dictionary<string, SkillTargeting> SpecialTargetings = new()
    {
        [Vivify] = new ChosenTarget(new FallenAllies()),
        [Revive] = new FallenAllies(),
        [ThousandSpears] = new SelectedOpponent(new LowestStatSelector(StatType.PhysDef)),
        [RainOfArrows] = new SelectedOpponent(new LowestStatSelector(StatType.CurrentHp)),
        [GuardianLiondog] = new SelectedOpponent(new HighestStatSelector(StatType.Speed))
    };

    public static ActiveSkill Create(SkillJson skill)
    {
        string name = Require(skill.Name, "una habilidad sin nombre");
        return new ActiveSkill(name, skill.SP, ParseTarget(name, skill), CreateEffects(name, skill));
    }

    private static List<Effect> CreateEffects(string name, SkillJson skill)
        => SpecialEffects.GetValueOrDefault(name)?.Invoke(skill) ?? CreateDamageEffects(skill);

    private static List<Effect> CreateShootingStarsEffects(SkillJson skill)
        => ShootingStarsTypes
            .Select(type => (Effect)new DamageEffect(AttackType.CreateNamed(type), skill.Modifier)).ToList();

    private static List<Effect> CreateDamageEffects(SkillJson skill)
        => DealsTypedDamage(skill)
            ? EffectRepetition.Repeat(CreateTypedDamage(skill), GetHits(skill))
            : new List<Effect>();

    private static List<Effect> CreateDrainingAttack(SkillJson skill, Effect drain)
        => CreateDamageEffects(skill).Append(drain).ToList();

    private static int GetHits(SkillJson skill)
        => HitsBySkill.GetValueOrDefault(Require(skill.Name, "una habilidad sin nombre"), SingleHit);

    private static List<Effect> CreateHealEffects(SkillJson skill)
        => AsList(new HealEffect(skill.Modifier));

    private static Effect CreateTypedDamage(SkillJson skill)
        => new DamageEffect(ParseType(skill), skill.Modifier);

    private static bool DealsTypedDamage(SkillJson skill)
        => skill.Type != null && AttackTypeNames.Exists(skill.Type);

    private static List<Effect> AsList(Effect effect)
        => new List<Effect> { effect };

    private static AttackType ParseType(SkillJson skill)
        => AttackType.CreateNamed(Require(skill.Type, $"el tipo de {skill.Name}"));

    private static SkillTargeting ParseTarget(string name, SkillJson skill)
        => SpecialTargetings.GetValueOrDefault(name) ?? ParseStandardTarget(skill);

    private static SkillTargeting ParseStandardTarget(SkillJson skill)
        => Targetings.GetValueOrDefault(Require(skill.Target, $"el objetivo de {skill.Name}"))
           ?? throw new InvalidDataException($"{skill.Name} objetivo desconocido");

    private static string Require(string? value, string missing)
        => value ?? throw new InvalidDataException($"falta {missing}");
}
