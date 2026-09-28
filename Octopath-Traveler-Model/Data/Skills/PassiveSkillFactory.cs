using Octopath_Traveler.Data.Json;
using Octopath_Traveler.Models.Skills;

namespace Octopath_Traveler.Data.Skills;

public static class PassiveSkillFactory
{
    public static PassiveSkill? Create(PassiveSkillJson passiveSkill)
    {
        Effect? effect = EffectFactory.CreateStatBonusEffect(passiveSkill);
        if (effect == null || passiveSkill.Name == null || passiveSkill.Target == null) return null;
        return new PassiveSkill(passiveSkill.Name, passiveSkill.Target, effect);
    }
}
