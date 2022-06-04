using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Common
{
    public class SeasonalEvents : ModSystem 
    {
        public static SeasonalEvents Instance;
        public static bool enabled = ConsolariaConfig.Instance.easterEnabled || ConsolariaConfig.Instance.thanksgivingEnabled || ConsolariaConfig.Instance.smallEventsEnabled,
            isEaster = ConsolariaConfig.Instance.easterEnabled && Helper.CheckEaster(),
            isThanksgiving = ConsolariaConfig.Instance.thanksgivingEnabled && Helper.CheckThanksgiving(),
            isChineseNewYear = ConsolariaConfig.Instance.smallEventsEnabled && Helper.CheckChineseNewYear(),
            isOktoberfest = ConsolariaConfig.Instance.smallEventsEnabled && Helper.CheckOktoberfest(),
            isSaintPatricksDay = ConsolariaConfig.Instance.smallEventsEnabled && Helper.CheckSaintPatricksDay();

        /*  public override void PostUpdateTime() {
              if (ConsolariaConfig.Instance.easterEnabled || ConsolariaConfig.Instance.thanksgivingEnabled) enabled = true;
              else return;
              if (ConsolariaConfig.Instance.easterEnabled && Helper.CheckEaster()) isEaster = true;
              else return;
              if (ConsolariaConfig.Instance.thanksgivingEnabled && Helper.CheckThanksgiving()) isThanksgiving = true;
              else return;
          }*/
    }
}