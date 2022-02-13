using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
	public class GoldenTurtle : ModProjectile
	{
        private int turnTimer;

        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 13;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.Turtle);
            AIType = ProjectileID.Turtle;

            int width = 44; int height = 28;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].turtle = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.GoldenTurtle>()))
                Projectile.timeLeft = 2;

            if (Projectile.velocity.Y != 0.4f) {
                if (Projectile.direction != player.direction) turnTimer++;
                if (turnTimer >= 60) {
                    Projectile.direction = player.direction;
                    turnTimer = 0;
                }
            }
        }

        private int texFrameCounter;
        private int texCurrentFrame;

        public override bool PreDraw(ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            bool onGround = Projectile.velocity.Y == 0f;
            texFrameCounter++;
            if (texFrameCounter >= 9) {
                texFrameCounter = 0;
                texCurrentFrame++;
                if (texCurrentFrame >= (onGround ? 8 : 12))
                    texCurrentFrame = onGround ? 0 : 9;
            }
            if (onGround && Projectile.velocity.X == 0f) {
                texCurrentFrame = 0;
                texFrameCounter = 0;
            }
            Vector2 position = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            var spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frameRect = new Rectangle(0, texCurrentFrame * frameHeight, texture.Width, frameHeight);
            spriteBatch.Draw(texture, position, frameRect, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}
