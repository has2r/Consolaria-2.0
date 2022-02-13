using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies
{
	public class Pointer : ModProjectile
	{
		public override string Texture => "Consolaria/Assets/Textures/Empty";

		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("");
		
		public override void SetDefaults() {
			int width = 10; int height = 20;
			Projectile.Size = new Vector2(width, height);

			Projectile.hostile = false;
			Projectile.friendly = false;

			Projectile.aiStyle = -1;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 80;
			Projectile.penetrate = -1;
		}	
		
		public override void AI() {
			int num;
			for (int num164 = 0; num164 < 1; num164 = num + 1) {
				float x2 = Projectile.position.X - Projectile.velocity.X / 10f * (float)num164;
				float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)num164;
				int num165 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 235, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num165].position.X = x2;
				Main.dust[num165].position.Y = y2;
				Dust dust3 = Main.dust[num165];
				dust3.velocity *= 0f;
				Main.dust[num165].noGravity = true;
				Main.dust[num165].scale = 1.3f;
				num = num164;
			}
		}
	}
}

			