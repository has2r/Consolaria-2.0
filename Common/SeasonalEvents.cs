using Terraria.ModLoader;

namespace Consolaria.Common
{
    public class SeasonalEvents : ModSystem 
    {
        public static SeasonalEvents Instance;
        public static bool enabled = ConsolariaConfig.Instance.easterEnabled || ConsolariaConfig.Instance.thanksgivingEnabled,
            isEaster = ConsolariaConfig.Instance.easterEnabled && Helper.CheckEaster(),
            isThanksgiving = ConsolariaConfig.Instance.thanksgivingEnabled && Helper.CheckThanksgiving();

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