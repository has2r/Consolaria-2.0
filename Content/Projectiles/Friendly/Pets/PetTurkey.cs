using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class PetTurkey : ModProjectile {
        public override void SetStaticDefaults () {
            Main.projFrames [Projectile.type] = 8;
            Main.projPet [Projectile.type] = true;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 4)
				.WithOffset(-13, 0)
				.WithSpriteDirection(1)
                .WhenNotSelected(0, 0);
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.BabyWerewolf);
            AIType = ProjectileID.BabyWerewolf;

            int width = 40; int height = 36;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI () {
            Main.player [Projectile.owner].petFlagBabyWerewolf = false;
            return true;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
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

        public override bool PreDraw (ref Color lightColor) {
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            bool onGround = Projectile.velocity.Y == 0f;
            texFrameCounter++;
            if (texFrameCounter >= 4) {
                texFrameCounter = 0;
                texCurrentFrame++;
                if (texCurrentFrame >= (onGround ? 4 : Main.projFrames [Projectile.type]))
                    texCurrentFrame = onGround ? 0 : 4;
            }
            if (onGround && Projectile.velocity.X == 0f) {
                texCurrentFrame = 0;
                texFrameCounter = 0;
            }

            Vector2 position = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            var spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int frameHeight = texture.Height / Main.projFrames [Projectile.type];
            Rectangle frameRect = new Rectangle(0, texCurrentFrame * frameHeight, texture.Width, frameHeight);
            Main.EntitySpriteDraw(texture, position, frameRect, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}