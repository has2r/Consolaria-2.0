using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs
{
	public class MythicalWyvernling : ModBuff
	{
		public override void SetStaticDefaults() {

			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex){ 
			player.buffTime[buffIndex] = 18000;
			ushort type = (ushort)ModContent.ProjectileType<Projectiles.Friendly.Pets.MythicalWyvernling>();

			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[type] <= 0)
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, type, 0, 0f, player.whoAmI);
		}
	}
}