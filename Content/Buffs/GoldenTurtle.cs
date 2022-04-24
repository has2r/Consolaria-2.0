using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs
{
	public class GoldenTurtle : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Golden Turtle");
			Description.SetDefault("Rare turtle");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex){ 
			player.buffTime[buffIndex] = 18000;
			sbyte type = (sbyte)ModContent.ProjectileType<Projectiles.Friendly.Pets.GoldenTurtle>();
			//var entitySource = player.GetSource_Buff(buffIndex);
			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[type] <= 0)
				Projectile.NewProjectile(player.GetProjectileSource_Buff(buffIndex), player.Center, Vector2.Zero, type, 0, 0f, player.whoAmI);
		}
	}
}