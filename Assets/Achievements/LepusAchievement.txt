using Consolaria.NPCs.Lepus;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;
using WebmilioCommons.Achievements;

namespace Consolaria.Achievements.LepusAchievement
{
    public class LepusAchievement : ModAchievement
    {
        public LepusAchievement() : base("Hare Despair", "Defeat Lepus, the greatest Easter troublemaker.", AchievementCategory.Slayer)
        {

        }
        public override void SetDefaults()
        {
            AddCondition(NPCKilledCondition.Create((short)ModContent.NPCType<Lepus>()));
        }
    }
}
