using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs
{
	public class WeirdTurkey : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Weird Turkey");
			// Description.SetDefault("Weird turkey will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Friendly.TurkeyHead>()] > 0)
				player.buffTime[buffIndex] = 18000;
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}