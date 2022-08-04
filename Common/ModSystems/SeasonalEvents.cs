using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Common {
    public class SeasonalEvents : ModSystem {
        public static bool allEventsForToday;
        public static bool enabled = ConsolariaConfig.Instance.easterEnabled || ConsolariaConfig.Instance.thanksgivingEnabled || ConsolariaConfig.Instance.smallEventsEnabled || allEventsForToday,
            isEaster = ConsolariaConfig.Instance.easterEnabled && SeasonalEventsHelper.CheckEaster(),
            isThanksgiving = ConsolariaConfig.Instance.thanksgivingEnabled && SeasonalEventsHelper.CheckThanksgiving(),
            isChineseNewYear = (ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckChineseNewYear()) || allEventsForToday,
            isOktoberfest = (ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckOktoberfest()) || allEventsForToday,
            isSaintPatricksDay = (ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckSaintPatricksDay()) || allEventsForToday,
            isValentinesDay = (ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckValentinesDay()) || allEventsForToday;

        public override void PostUpdateTime () {
            if (Main.time == 24000.0) {
                if (allEventsForToday) {
                    allEventsForToday = false;
                    Main.NewText("Events is over!", Color.HotPink);
                }
            }
        }
    }
}