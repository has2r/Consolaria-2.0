using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
	public class LilMarco : ModProjectile
	{
        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
        }
        
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);                      
            AIType = ProjectileID.ZephyrFish;

            int width = 44; int height = 40;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].zephyrfish = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.active) {
                Projectile.active = false;
                return;
            }
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.LilMarco>()))
                Projectile.timeLeft = 2;

            Projectile.velocity.X *= 1.025f;
            Projectile.rotation += Projectile.velocity.X / 15f;
        }

        private int texFrameCounter;
        private int texCurrentFrame;

        public override bool PreDraw(ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            texFrameCounter++;
            if (texFrameCounter > 8) {
                texCurrentFrame++;
                texFrameCounter = 0;
            }
            if (texCurrentFrame >= Main.projFrames[Projectile.type])
                texCurrentFrame = 0;
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
