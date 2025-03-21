using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.IO;

using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Slime : ModProjectile {
        public override void SetStaticDefaults () {
            Main.projFrames [Projectile.type] = 6;
            Main.projPet [Projectile.type] = true;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 6)
				.WithOffset(-2, 0)
				.WithSpriteDirection(1)
                .WhenNotSelected(0, 0);
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

        private int choosenBalloon = 0;

        public override void SendExtraAI (BinaryWriter writer)
            => writer.Write(choosenBalloon);

        public override void ReceiveExtraAI (BinaryReader reader)
            => choosenBalloon = reader.ReadInt32();

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Slime>()))
                Projectile.timeLeft = 2;

            if (choosenBalloon == 0)

                choosenBalloon = Main.rand.NextFromList(1, 2, 3, 4, 5, 6);


            Projectile.frameCounter = 0;
            Projectile.frame = 8;
        }

        private int texFrameCounter;
        private int texCurrentFrame;

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            int framesCountX = 7;
            SpriteFrame frame = new SpriteFrame((byte) framesCountX, 1);
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

            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            var spriteEffects = Projectile.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int frameHeight = texture.Height / Main.projFrames [Projectile.type];
            Rectangle frameRect = new Rectangle(0, texCurrentFrame * frameHeight, texture.Width, frameHeight);
            int offsetY = 8;

            Player player = Main.player [Projectile.owner];
            Color slimeColor = Main.tenthAnniversaryWorld || Main.drunkWorld || Main.getGoodWorld || Main.zenithWorld ? Main.DiscoColor : player.shirtColor;

            int intendedShader = player.cPet;
            spriteBatch.End();
            var matrix = Main.gameMenu ? Main.UIScaleMatrix : Main.GameViewMatrix.TransformationMatrix;
            var samplerMode = Main.gameMenu ? SamplerState.AnisotropicClamp : Main.DefaultSamplerState;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerMode, DepthStencilState.None, default, default, matrix);

            DrawData value = new(texture, new Vector2(position.X, position.Y - offsetY), frameRect, slimeColor.MultiplyRGB(lightColor) * 0.8f, 0, drawOrigin, Projectile.scale, spriteEffects, 0f);
            GameShaders.Armor.Apply(intendedShader, Projectile, value);
            value.Draw(spriteBatch);

            if (isFlying) {
                Rectangle rectangle = frame.GetSourceRectangle(balloon);
                int width = balloon.Width / framesCountX;
                rectangle.X = width * (choosenBalloon - 1);
                spriteBatch.Draw(balloon, new Vector2(position.X, position.Y - offsetY - 62 + 14) + new Vector2(6, 4), rectangle, lightColor, 0, drawOrigin, Projectile.scale, SpriteEffects.None, 1f);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerMode, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, matrix);
            return false;
        }
    }
}