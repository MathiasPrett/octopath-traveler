using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class BeastSkillFactory
{
    private const string VortalClaw = "Vortal Claw";
    private const string PhysicalAttack = "físico";
    private const string EveryTraveler = "Enemies";

    private static readonly Dictionary<string, StatType> HighestStatTargets = new()
    {
        ["mayor HP"] = StatType.CurrentHp,
        ["mayor Elem Atk"] = StatType.ElemAtk,
        ["mayor Phys Atk"] = StatType.PhysAtk,
        ["mayor Phys Def"] = StatType.PhysDef,
        ["mayor Speed"] = StatType.Speed
    };

    private static readonly Dictionary<string, StatType> LowestStatTargets = new()
    {
        ["menor Phys Def"] = StatType.PhysDef,
        ["menor Elem Def"] = StatType.ElemDef,
        ["menor Speed"] = StatType.Speed
    };

    public static BeastSkill Create(BeastSkillJson skill)
        => new BeastSkill(RequireName(skill), CreateSelector(skill), CreateEffects(skill));

    private static TargetSelector CreateSelector(BeastSkillJson skill)
    {
        if (skill.Target == EveryTraveler) return new EveryCandidateSelector();
        string description = RequireDescription(skill);
        return CreateHighestStatSelector(description) ?? CreateLowestStatSelector(description)
               ?? throw new InvalidDataException($"{skill.Name} sin objetivo");
    }

    private static TargetSelector? CreateHighestStatSelector(string description)
        => FindStatIn(HighestStatTargets, description) is StatType stat ? new HighestStatSelector(stat) : null;

    private static TargetSelector? CreateLowestStatSelector(string description)
        => FindStatIn(LowestStatTargets, description) is StatType stat ? new LowestStatSelector(stat) : null;

    private static StatType? FindStatIn(Dictionary<string, StatType> targets, string description)
        => targets.Where(pair => description.Contains(pair.Key))
            .Select(pair => (StatType?)pair.Value).FirstOrDefault();

    private static List<Effect> CreateEffects(BeastSkillJson skill)
        => Repeat(RequireName(skill) == VortalClaw ? new HalveHpEffect() : CreateDamageEffect(skill), skill.Hits);

    private static Effect CreateDamageEffect(BeastSkillJson skill)
        => new DamageEffect(DeriveType(skill), skill.Modifier);

    private static AttackType DeriveType(BeastSkillJson skill)
        => RequireDescription(skill).Contains(PhysicalAttack) ? AttackType.Physical() : AttackType.Elemental();

    private static List<Effect> Repeat(Effect effect, int hits)
        => Enumerable.Repeat(effect, hits).ToList();

    private static string RequireName(BeastSkillJson skill)
        => skill.Name ?? throw new InvalidDataException("skill sin nombre");

    private static string RequireDescription(BeastSkillJson skill)
        => skill.Description ?? throw new InvalidDataException($"{skill.Name} sin descripción");
}
