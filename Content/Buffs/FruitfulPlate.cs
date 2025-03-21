using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs {
    public class FruitfulPlate : ModBuff
	{
		public override void SetStaticDefaults() {

			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex){
			player.mount.SetMount(ModContent.MountType<Mounts.FruitfulPlateMount>(), player);
			player.buffTime[buffIndex] = 10; 
		}
	}
}