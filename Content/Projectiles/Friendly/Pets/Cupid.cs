using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
	public class Cupid : ModProjectile
	{
        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.LightPet[Projectile.type] = true;
        }
        
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);                      
            AIType = ProjectileID.ZephyrFish;

            int width = 38; int height = 50;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].zephyrfish = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Cupid>()))
                Projectile.timeLeft = 2;

            Projectile.velocity *= 0.985f;
            if (!Main.dedServ)
                Lighting.AddLight(Projectile.Center, new Color(255, 105, 180).ToVector3() * 0.7f);
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
            var spriteEffects = Projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frameRect = new Rectangle(0, texCurrentFrame * frameHeight, texture.Width, frameHeight);
            spriteBatch.Draw(texture, position, frameRect, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}
