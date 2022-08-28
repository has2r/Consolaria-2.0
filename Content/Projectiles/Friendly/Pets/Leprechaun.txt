using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Leprechaun : ModProjectile {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Leprechaun O'Fyffe");

            Main.projFrames [Projectile.type] = 9;
            Main.projPet [Projectile.type] = true;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.Penguin);
            AIType = ProjectileID.Penguin;

            int width = 30; int height = 50;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI () {
            Main.player [Projectile.owner].penguin = false;
            return true;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Leprechaun>()))
                Projectile.timeLeft = 2;

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