using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class EternalScythe : ModProjectile {
		private float rotationTimer = (float) Math.PI;
		public override void SetDefaults () {
			int width = 32; int height = width;
			Projectile.Size = new Vector2(width, height);

			Projectile.hostile = false;
			Projectile.friendly = true;

			Projectile.DamageType = DamageClass.Summon;
			Projectile.scale = 0.25f;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 250;
		}

		public override void AI () {
			if (Projectile.scale < 0.65f)
				Projectile.scale += 0.1f;

			if (Projectile.timeLeft > 200)
				Projectile.velocity *= 1.05f;

			if (Main.netMode != NetmodeID.Server) {
				if (Main.rand.NextBool(3))
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 0.8f);
			}

			Projectile.rotation += 2 / rotationTimer;
			rotationTimer += 0.01f;
		}

		public override void OnHitNPC (NPC target, NPC.HitInfo hit, int damageDone)
			=> target.AddBuff(BuffID.ShadowFlame, 180);

		public override void OnHitPlayer (Player target, Player.HurtInfo info) {
			if (info.PvP)
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

		public override Color? GetAlpha (Color lightColor)
			=> Color.White;
	}
}