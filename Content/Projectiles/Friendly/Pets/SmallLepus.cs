using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class SmallLepus : ModProjectile {
        private float rotationSpeed;
        private bool isFlying;

        public override void SetStaticDefaults () {

            Main.projFrames [Projectile.type] = 5;
            Main.projPet [Projectile.type] = true;

        ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 3)
				.WithOffset(-13, 0)
				.WithSpriteDirection(1)
                .WhenNotSelected(0, 0); //I don't understand why tf tModloader is ignoring this line when it's directly copied from ExampleMod
        }

        public override void SetDefaults () {
            int width = 36; int height = 36;
            Projectile.Size = new Vector2(width, height);

            AIType = ProjectileID.KingSlimePet;
            Projectile.aiStyle = 26;

            Projectile.netImportant = true;
            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
        }

        public override bool PreAI () {
            Main.player [Projectile.owner].petFlagKingSlimePet = false;
            return true;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.SmallLepus>()))
                Projectile.timeLeft = 2;

            isFlying = Projectile.ai [0] == 1;

            //replacing vanilla king slime pet gore
            Projectile.localAI [0]++;
            Projectile.frameCounter = 0;
            Projectile.frame = 8;

            if (Projectile.localAI [0] == (60 * Main.rand.Next(6, 15)) && !isFlying) {
                if (Main.netMode != NetmodeID.Server)
                    Gore.NewGore(Projectile.GetSource_FromAI(), new Vector2(Projectile.position.X, Projectile.Center.Y), Vector2.Zero, ModContent.Find<ModGore>("Consolaria/EasterEggFullGore").Type);
                Projectile.localAI [0] = 0;
            }

            if (isFlying) rotationSpeed += Projectile.velocity.X * 0.1f;
            else rotationSpeed = 0;

            if (Projectile.velocity.Y != 0.4f) {
                if (Projectile.direction != player.direction)
                    Projectile.direction = player.direction;
            }
        }

        private int texFrameCounter;
        private int texCurrentFrame;

        public override bool PreDraw (ref Color lightColor) {
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            bool fuckingGround = Projectile.velocity.X == 0f;
            texFrameCounter++;
            if (isFlying) {
                texCurrentFrame = 4;
                texFrameCounter = 0;
            }
            else if (texFrameCounter >= (fuckingGround ? 6 : 8)) {
                texFrameCounter = 0;
                texCurrentFrame++;
                if (texCurrentFrame >= (fuckingGround ? 2 : 4))
                    texCurrentFrame = 0;
            }

            Vector2 position = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            var spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int frameHeight = texture.Height / Main.projFrames [Projectile.type];
            Rectangle frameRect = new Rectangle(0, texCurrentFrame * frameHeight, texture.Width, frameHeight);
            Main.EntitySpriteDraw(texture, position, frameRect, lightColor, rotationSpeed, drawOrigin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}