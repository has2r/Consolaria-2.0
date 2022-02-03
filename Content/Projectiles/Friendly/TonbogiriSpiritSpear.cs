using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{	
	public class TonbogiriSpiritSpear : ModProjectile
	{	
		public override void SetDefaults() {
			int width = 20; int height = width;
			Projectile.Size = new Vector2(width, height);

			Projectile.aiStyle = 19;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
		}		
		
		public float MovementFactor {
			get { return Projectile.ai[0]; }
			set { Projectile.ai[0] = value; }
		}	

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 136, default, 1.2f);
            Main.dust[dust].noGravity = true;
			Projectile.scale = Projectile.ai[1];

			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
			Projectile.direction = player.direction;
			player.heldProj = Projectile.whoAmI;
			player.itemTime = player.itemAnimation;
			Projectile.position.X = vector.X - (Projectile.width / 2);
			Projectile.position.Y = vector.Y - (Projectile.height / 2);

			if (!player.frozen) {
				if (MovementFactor == 0f) {
					MovementFactor = 3f;
					Projectile.netUpdate = true;
				}
				MovementFactor -= ((player.itemAnimation < player.itemAnimationMax / 4) ? 1.5f : -1.5f);
			}

			Projectile.position += Projectile.velocity * MovementFactor;
			if (player.itemAnimation == 0) Projectile.Kill();
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(135f);
			if (Projectile.spriteDirection == -1) Projectile.rotation -= MathHelper.ToRadians(90f);	
		}

		public override void Kill(int timeLeft) {
			if (Projectile.owner == Main.myPlayer)
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 68, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f);		
        }

		public override Color? GetAlpha(Color lightColor)
			=> new Color(Color.White.R, Color.White.G, Color.White.B, 0) * (1f - (float)Projectile.alpha / 255f);
	}
}
