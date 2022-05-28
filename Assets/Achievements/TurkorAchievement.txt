using Consolaria.NPCs.Turkor;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;
using WebmilioCommons.Achievements;

namespace Consolaria.Achievements.TurkorAchievement
{
    public class TurkorAchievement : ModAchievement
    {
        public TurkorAchievement() : base("I Like Large Fries, But Not Fried Chicken", "Defeat Turkor, the burning Thanksgiving monstrosity.", AchievementCategory.Slayer)
        {

        }
        public override void SetDefaults()
        {
            AddCondition(NPCKilledCondition.Create((short)ModContent.NPCType<TurkortheUngrateful>()));
        }
    }
}
