using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Slime : ModProjectile {
        public override void SetStaticDefaults () {
            Main.projFrames [Projectile.type] = 6;
            Main.projPet [Projectile.type] = true;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.KingSlimePet);
            AIType = ProjectileID.KingSlimePet;

            int width = 30; int height = 22;
            Projectile.Size = new Vector2(width, height);
            Projectile.alpha = byte.MaxValue;
        }

        public override bool PreAI () {
            Main.player [Projectile.owner].petFlagKingSlimePet = false;
            return true;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Slime>()))
                Projectile.timeLeft = 2;

            Projectile.frameCounter = 0;
            Projectile.frame = 8;
        }

        private int texFrameCounter;
        private int texCurrentFrame;

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            Texture2D balloon = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/SlimePet_Balloon");

            bool isFlying = Projectile.ai [0] == 1;
            texFrameCounter++;
            if (isFlying) {
                texCurrentFrame = 5;
                texFrameCounter = 0;
            }
            else if (texFrameCounter >= 10) {
                texFrameCounter = 0;
                texCurrentFrame++;
                if (texCurrentFrame >= 4)
                    texCurrentFrame = 0;
            }

            Vector2 position = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            var spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int frameHeight = texture.Height / Main.projFrames [Projectile.type];
            Rectangle frameRect = new Rectangle(0, texCurrentFrame * frameHeight, texture.Width, frameHeight);
            int offsetY = 8;
            spriteBatch.Draw(texture, new Vector2(position.X, position.Y - offsetY), frameRect, Main.DiscoColor * 0.8f, 0, drawOrigin, Projectile.scale, spriteEffects, 0f);
            if (isFlying)
                spriteBatch.Draw(balloon, new Vector2(position.X, position.Y - offsetY - 62), null, lightColor, 0, drawOrigin, Projectile.scale, spriteEffects, 1f);
            return false;
        }
    }
}