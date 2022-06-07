using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
    public class SmallLepus : ModProjectile
    {
        private float ratationSpeed;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Baby Lepus");

            Main.projFrames[Projectile.type] = 5;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.KingSlimePet);
            AIType = ProjectileID.KingSlimePet;

            int width = 38; int height = 36;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].petFlagKingSlimePet = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.SmallLepus>()))
                Projectile.timeLeft = 2;

            if (Projectile.ai[0] == 1) ratationSpeed += Projectile.velocity.X * 0.1f;
            else ratationSpeed = 0;

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

            bool fackingGround = Projectile.velocity.X == 0f;
            texFrameCounter++;
            if (Projectile.ai[0] == 1) {
                texCurrentFrame = 4;
                texFrameCounter = 0;
            }
            else if (texFrameCounter >= (fackingGround ? 6 : 8)) {
                texFrameCounter = 0;
                texCurrentFrame++;
                if (texCurrentFrame >= (fackingGround ? 2 : 4))
                    texCurrentFrame = 0;
            }
            
            Vector2 position = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            var spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frameRect = new Rectangle(0, texCurrentFrame * frameHeight, texture.Width, frameHeight);
            spriteBatch.Draw(texture, position, frameRect, lightColor, ratationSpeed, drawOrigin, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}
