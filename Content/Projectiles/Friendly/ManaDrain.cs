using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
	public class ManaDrain : ModProjectile {
		public override string Texture => "Consolaria/Assets/Textures/Empty";

		public override void SetDefaults () {
			int width = 4; int height = width;
			Projectile.Size = new Vector2(width, height);

			Projectile.penetrate = -1;

			Projectile.hostile = false;
			Projectile.friendly = true;

			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			Projectile.extraUpdates = 15;
		}

		public override void AI () {
			Player player = Main.player [Main.myPlayer];

			float speed = 4f;
			Vector2 velocity = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float posX = player.Center.X - velocity.X;
			float posY = player.Center.Y - velocity.Y;
			float position = (float) Math.Sqrt((posX * posX + posY * posY));
			if (position < 50f && Projectile.position.X < player.position.X + player.width && Projectile.position.X + Projectile.width > player.position.X && Projectile.position.Y < player.position.Y + player.height && Projectile.position.Y + Projectile.height > player.position.Y) {
				if (Projectile.owner == Main.myPlayer) {
					int healMana = Main.rand.Next(1, 10);
					player.ManaEffect(healMana);
					player.statMana += healMana;
				}
				SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, Projectile.Center);
				Projectile.Kill();
			}
			position = speed / position;
			posX *= position;
			posY *= position;
			Projectile.velocity.X = (Projectile.velocity.X * 15f + posX) / 16f;
			Projectile.velocity.Y = (Projectile.velocity.Y * 15f + posY) / 16f;

			for (int dustCount = 0; dustCount < 3; dustCount++) {
				float velX = Projectile.velocity.X * 0.3f * dustCount;
				float velY = -(Projectile.velocity.Y * 0.3f) *  dustCount;
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, default, 1.1f);
				Main.dust [dust].noGravity = true;
				Main.dust [dust].velocity *= 0f;
				Dust expr_15516_cp_0 = Main.dust [dust];
				expr_15516_cp_0.position.X = expr_15516_cp_0.position.X - velX;
				Dust expr_15535_cp_0 = Main.dust [dust];
				expr_15535_cp_0.position.Y = expr_15535_cp_0.position.Y - velY;
			}
		}

		public override bool? CanCutTiles () 
			=> false;

		public override bool? CanDamage ()
			=> false;
    }
}