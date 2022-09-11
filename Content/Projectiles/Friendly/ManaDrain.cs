using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
	public class ManaDrain : ModProjectile {
		private int cycle;
		private bool cycleSwitch;
		private int bonusHealMana;

		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults () {
			int width = 1; int height = width;
			Projectile.Size = new Vector2(width, height);

			Projectile.penetrate = -1;

			Projectile.hostile = false;
			Projectile.friendly = true;

			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			Projectile.extraUpdates = 10;
		}

		public override void AI () {
			Player player = Main.player [Main.myPlayer];
			float speed = 3f;
			Vector2 velocity = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float posX = player.Center.X - velocity.X;
			float posY = player.Center.Y - velocity.Y;
			float position = (float) Math.Sqrt((posX * posX + posY * posY));
			if (position < 50f && Projectile.position.X < player.position.X + player.width && Projectile.position.X + Projectile.width > player.position.X && Projectile.position.Y < player.position.Y + player.height && Projectile.position.Y + Projectile.height > player.position.Y) {
				if (Projectile.owner == Main.myPlayer) {
					bonusHealMana = 0;
					if (player.manaRegenBonus > 0) bonusHealMana++;
					if (player.manaRegenBonus > 20) bonusHealMana++;
					if (player.manaRegenBonus > 35) bonusHealMana++;
					if (player.HasBuff(BuffID.ManaRegeneration)) bonusHealMana++;
					int healMana = Main.rand.Next(4, 10) + bonusHealMana;
					player.ManaEffect(healMana);
					player.statMana += healMana;
					NetMessage.SendData(MessageID.ManaEffect, -1, -1, null, Projectile.owner, healMana);
				}
				Projectile.Kill();
			}
			position = speed / position;
			posX *= position;
			posY *= position;
			Projectile.velocity.X = (Projectile.velocity.X * 15f + posX) / 16f;
			Projectile.velocity.Y = (Projectile.velocity.Y * 15f + posY) / 16f;

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (!cycleSwitch) cycle += 2;
			else cycle--;
			if (cycle >= 80) cycleSwitch = true;
			if (cycle <= 0) cycleSwitch = false;

			if (Main.netMode != NetmodeID.Server) {
				if (Main.rand.NextBool(9)) {
					float velX = Projectile.velocity.X * 0.3f;
					float velY = -Projectile.velocity.Y * 0.3f;
					int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.RainbowMk2, velX, velY, 120, new Color(240 - cycle * 2, 225 - cycle * 2, cycle * 3, 50), 0.6f);
					Main.dust [dust2].velocity *= 0.25f;
					Main.dust [dust2].noLightEmittence = true;
					Main.dust [dust2].noGravity = true;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Color color = new Color(240 - cycle * 2, 225 - cycle * 2, cycle * 3, 50);
				float rotation = (float)Math.Atan2(Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y, Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X);
                spriteBatch.Draw(texture, drawPos, null, color * 0.3f, rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length, effects, 0f);
            }
            return false;
        }

		public override bool? CanCutTiles () 
			=> false;

		public override bool? CanDamage ()
			=> false;
    }
}