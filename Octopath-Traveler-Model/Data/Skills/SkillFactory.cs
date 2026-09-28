using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class SkillFactory
{
    public static ActiveSkill? Create(SkillJson skill)
    {
        Effect? effect = EffectFactory.CreateDamageEffect(skill);
        if (effect == null || skill.Name == null || skill.Target == null) return null;
        return new ActiveSkill(skill.Name, skill.SP, skill.Target, ConditionFactory.Create(skill), effect);
    }
}
