using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Models;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class EffectFactory
{
    // passive_skills.json describe sus efectos en texto libre, así que los bonus de stat
    // se registran acá hasta que el archivo traiga campos estructurados.
    private static readonly Dictionary<string, Effect> StatBonuses = new()
    {
        ["Elemental Augmentation"] = new BonusEffect(StatType.ElemAtk, 50),
        ["Summon Strength"] = new BonusEffect(StatType.PhysAtk, 50),
        ["Hale and Hearty"] = new BonusEffect(StatType.HpMax, 500),
        ["Fleefoot"] = new BonusEffect(StatType.Speed, 50)
    };

    public static Effect? CreateDamageEffect(SkillJson skill)
    {
        AttackCategory? category = AttackTypeCatalog.Classify(skill.Type);
        if (category == null) return null;
        return new DamageEffect(category.Value, skill.Modifier);
    }

    public static Effect? CreateStatBonusEffect(PassiveSkillJson passiveSkill)
        => passiveSkill.Name != null && StatBonuses.TryGetValue(passiveSkill.Name, out Effect? effect)
            ? effect
            : null;
}
