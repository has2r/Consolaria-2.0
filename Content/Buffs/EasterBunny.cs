using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs {
    public class EasterBunny : ModBuff {
        public override void SetStaticDefaults() {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Friendly.EasterBunny>()] > 0)
                player.buffTime[buffIndex] = 18000;
            else {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}