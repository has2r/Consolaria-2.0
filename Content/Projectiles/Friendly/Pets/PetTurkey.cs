using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
    public class PetTurkey : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Turkey");
            Main.projFrames[Projectile.type] = 8;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.BabyWerewolf);
            AIType = ProjectileID.BabyWerewolf;

            int width = 40; int height = 36;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].petFlagBabyWerewolf = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.PetTurkey>()))
                Projectile.timeLeft = 2;

            Projectile.rotation = 0;
            if (Projectile.velocity.Y != 0.4f) {
                if (Projectile.direction != player.direction)
                    Projectile.direction = player.direction;
            }
        }
        
        private int texFrameCounter;
        private int texCurrentFrame;

        public override bool PreDraw(ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            bool onGround = Projectile.velocity.Y == 0f;
            texFrameCounter++;
            if (texFrameCounter >= 4) {
                texFrameCounter = 0;
                texCurrentFrame++;
                if (texCurrentFrame >= (onGround ? 4 : 8))
                    texCurrentFrame = onGround ? 0 : 4;
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
