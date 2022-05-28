using Consolaria.NPCs.Ocram;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;
using WebmilioCommons.Achievements;

namespace Consolaria.Achievements.OcramAchievement
{
    public class OcramAchievement : ModAchievement
    {
        public OcramAchievement() : base("Bible of the Beast", "Defeat Ocram, the cursed fiend haunting the starless sky.", AchievementCategory.Slayer)
        {

        }
        public override void SetDefaults()
        {
            AddCondition(NPCKilledCondition.Create((short)ModContent.NPCType<Ocram>()));
        }
    }
}
