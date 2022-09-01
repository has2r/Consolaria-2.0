using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Leprechaun : ConsolariaPet {
        public override int maxFrames => 9;

        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;

            base.SetStaticDefaults();
        }

        public override void SetDefaults () {
            int width = 30; int height = 50;
            Projectile.Size = new Vector2(width, height);

            DrawOffsetX -= 10;

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Leprechaun>()))
                Projectile.timeLeft = 2;

            WalkerAI();
            PassiveAnimation(idleFrame: 0, jumpFrame: 3);
            int finalFrame = maxFrames - 1;
            WalkingAnimation(walkingAnimationSpeed: 3, walkingFirstFrame: 0, finalFrame);
            FlyingAnimation(oneFrame: true);

            Projectile.localAI [0]++;
            if (Projectile.localAI [0] % 1800 == 0 && Projectile.velocity.X != 0)
                DropRandomCoin();
			
			double rotation = (Math.PI / 2) * Projectile.velocity.X * 0.08f;
            if (isFlying)
            Projectile.rotation = (float) rotation;
        }

        private void DropRandomCoin () {
            SoundEngine.PlaySound(SoundID.Coins with { Volume = 0.8f }, Projectile.Center);
            int coinType = Main.rand.Next(4);
            if (coinType == 0) Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.CopperCoin, 1);
            if (coinType == 1) {
                if (Main.rand.NextBool(25))
                    Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.SilverCoin, 1);
                else Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.CopperCoin, 1);
            }
            if (coinType == 2) {
                if (Main.rand.NextBool(100))
                    Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.GoldCoin, 1);
                else Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.CopperCoin, 1);
            }
            if (coinType == 3) {
                if (Main.rand.NextBool(300))
                    Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.PlatinumCoin, 1);
                else Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.CopperCoin, 1);
            }
        }

        public override bool PreDraw (ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/Leprechaun_Rainbow");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++) {
                Vector2 drawPos = Projectile.oldPos [k] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
                Color color = new Color(150, 150, 150, 50);
                float rotation = (float) Math.Atan2(Projectile.oldPos [k].Y - Projectile.oldPos [k + 1].Y, Projectile.oldPos [k].X - Projectile.oldPos [k + 1].X);
                if (isFlying)
                    spriteBatch.Draw(texture, new Vector2(drawPos.X - 10 * Projectile.direction, drawPos.Y + 20), null, color, rotation, drawOrigin, Projectile.scale - k / (float) Projectile.oldPos.Length + 0.5f, effects, 0f);
            }
            return true;
        }
    }
}