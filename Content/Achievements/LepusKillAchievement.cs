using Consolaria.Content.NPCs.Bosses.Lepus;

using Terraria.ModLoader;

namespace Consolaria.Content.Achievements;

public sealed class LepusKillAchievement : ModAchievement {
    public override void SetStaticDefaults() {
        Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Slayer);

        AddNPCKilledCondition(ModContent.NPCType<Lepus>());
    }

    public override Position GetDefaultPosition() => new Before("EYE_ON_YOU");

    public override Position GetAdvisorPosition() => new Before("EYE_ON_YOU");
}
