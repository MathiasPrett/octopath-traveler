using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class BeastSkillFactory
{
    private const string VortalClaw = "Vortal Claw";
    private const string PhysicalAttack = "físico";
    private const string EveryTraveler = "Enemies";

    // beast_skills.json no trae el tipo ni el criterio de objetivo: los dos salen de
    // la descripción.
    private static readonly Dictionary<string, StatType> HighestStatTargets = new()
    {
        ["mayor HP"] = StatType.HpCurrent,
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
        => new BeastSkill(NameOf(skill), SelectorFor(skill), new EffectSet(EffectsFor(skill)));

    private static TargetSelector SelectorFor(BeastSkillJson skill)
    {
        if (skill.Target == EveryTraveler) return new EveryCandidate();
        string description = DescriptionOf(skill);
        if (StatIn(HighestStatTargets, description, out StatType highest)) return new HighestStat(highest);
        if (StatIn(LowestStatTargets, description, out StatType lowest)) return new LowestStat(lowest);
        throw new InvalidDataException($"No se entiende a quién ataca {skill.Name}");
    }

    private static bool StatIn(Dictionary<string, StatType> targets, string description, out StatType stat)
    {
        KeyValuePair<string, StatType> match = targets.FirstOrDefault(pair => description.Contains(pair.Key));
        stat = match.Value;
        return match.Key != null;
    }

    private static List<Effect> EffectsFor(BeastSkillJson skill)
        => Repeat(NameOf(skill) == VortalClaw ? new HalveHpEffect() : DamageFor(skill), skill.Hits);

    private static Effect DamageFor(BeastSkillJson skill)
        => new DamageEffect(TypeOf(skill), skill.Modifier);

    private static AttackType TypeOf(BeastSkillJson skill)
        => DescriptionOf(skill).Contains(PhysicalAttack) ? AttackType.Physical() : AttackType.Elemental();

    private static List<Effect> Repeat(Effect effect, int hits)
        => Enumerable.Repeat(effect, hits).ToList();

    private static string NameOf(BeastSkillJson skill)
        => skill.Name ?? throw new InvalidDataException("beast_skills.json trae una habilidad sin nombre");

    private static string DescriptionOf(BeastSkillJson skill)
        => skill.Description ?? throw new InvalidDataException($"{skill.Name} no trae descripción");
}
