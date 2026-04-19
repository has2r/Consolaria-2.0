using Consolaria.Content.NPCs.Bosses.Ocram;

using Terraria.ModLoader;

namespace Consolaria.Content.Achievements;

public sealed class OcramKillAchievement : ModAchievement {
    public override void SetStaticDefaults() {
        Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Slayer);

        AddNPCKilledCondition(ModContent.NPCType<Ocram>());
    }

    public override Position GetDefaultPosition() => new After("THE_GREAT_SOUTHERN_PLANTKILL");

    public override Position GetAdvisorPosition() => new After("THE_GREAT_SOUTHERN_PLANTKILL");
}
