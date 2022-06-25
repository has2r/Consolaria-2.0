using Terraria.ModLoader;

namespace Consolaria.Common {
    public class SeasonalEvents : ModSystem {
        public static SeasonalEvents Instance;
        public static bool enabled = ConsolariaConfig.Instance.easterEnabled || ConsolariaConfig.Instance.thanksgivingEnabled || ConsolariaConfig.Instance.smallEventsEnabled,
            isEaster = ConsolariaConfig.Instance.easterEnabled && SeasonalEventsHelper.CheckEaster(),
            isThanksgiving = ConsolariaConfig.Instance.thanksgivingEnabled && SeasonalEventsHelper.CheckThanksgiving(),
            isChineseNewYear = ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckChineseNewYear(),
            isOktoberfest = ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckOktoberfest(),
            isSaintPatricksDay = ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckSaintPatricksDay(),
            isValentinesDay = ConsolariaConfig.Instance.smallEventsEnabled && SeasonalEventsHelper.CheckValentinesDay();
    }
}