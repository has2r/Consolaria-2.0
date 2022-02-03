using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs
{
	public class EyeOfEternity : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Eye of Eternity");
			Description.SetDefault("Eye of Eternity will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Friendly.EyeOfEternity>()] > 0)
				player.buffTime[buffIndex] = 18000;
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}