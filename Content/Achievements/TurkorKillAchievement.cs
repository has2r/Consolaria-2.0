using Consolaria.Content.NPCs.Bosses.Lepus;
using Consolaria.Content.NPCs.Bosses.Turkor;

using Terraria.ModLoader;

namespace Consolaria.Content.Achievements;

public sealed class TurkorKillAchievement : ModAchievement {
    public override void SetStaticDefaults() {
        Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Slayer);

        AddNPCKilledCondition(ModContent.NPCType<TurkortheUngrateful>());
    }

    public override Position GetDefaultPosition() => new After("DUNGEON_HEIST");

    public override Position GetAdvisorPosition() => new After("DUNGEON_HEIST");
}
