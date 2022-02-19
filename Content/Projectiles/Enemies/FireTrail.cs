using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies
{
	public class FireTrail : ModProjectile
	{
		public override string Texture => "Consolaria/Assets/Textures/Empty";

		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Fire Trail");
		
		public override void SetDefaults() {
			int width = 8; int height = width;
			Projectile.Size = new Vector2(width, height);

			Projectile.hostile = true;
			Projectile.friendly = false;

			Projectile.penetrate = -1;
			Projectile.tileCollide = true;

			Projectile.light = 0.25f;

			Projectile.timeLeft = 90;
			Projectile.extraUpdates = 1;
		}

		public override void AI() {
			if (Projectile.timeLeft <= 85 && Main.rand.Next(2) == 0) {
				int _dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default, 1f);
				Main.dust[_dust].noGravity = true;
				Main.dust[_dust].velocity *= 0.5f;
				Main.dust[_dust].scale = 1f;
			}
		}
		
		public override void OnHitPlayer(Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.OnFire, 180);
	}
}