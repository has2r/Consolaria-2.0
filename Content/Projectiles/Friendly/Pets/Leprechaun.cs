using Consolaria.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Leprechaun : ConsolariaPet {
        public override int maxFrames => 9;

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
    }
}