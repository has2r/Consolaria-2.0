using Microsoft.Xna.Framework;
using System;
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

            int width = 30; int height = 48;
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
            if (Projectile.localAI [0] % 1800 == 0)
                DropRandomCoin();
        }

       /* private void CheckJump () {
            Player player = Main.player [Projectile.owner];

            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;

            int num1 = 85;
            if (player.position.X + (double) (player.width / 2) < Projectile.position.X + (double) (Projectile.width / 2) - num1)
                flag1 = true;
            else if (player.position.X + (double) (player.width / 2) > Projectile.position.X + (double) (Projectile.width / 2) + num1)
                flag2 = true;

            if (Projectile.ai [1] == 0) {
                int num2 = 500;
                if (player.rocketDelay2 > 0)
                    Projectile.ai [0] = 1f;
                Vector2 vector2 = new Vector2(Projectile.position.X + (float) Projectile.width * 0.5f, Projectile.position.Y + (float) Projectile.height * 0.5f);
                double num3 = player.position.X + (double) (player.width / 2) - vector2.X;
                float num4 = player.position.Y + (float) (player.height / 2) - vector2.Y;
                float num5 = (float) Math.Sqrt(num3 * num3 + num4 * num4);
                if (num5 > 2000) {
                    Projectile.position.X = player.position.X + (float) (player.width / 2) - (float) (Projectile.width / 2);
                    Projectile.position.Y = player.position.Y + (float) (player.height / 2) - (float) (Projectile.height / 2);
                }
                else if (num5 > num2 || Math.Abs(num4) > 300 && Projectile.localAI [0] <= 0)
                    Projectile.ai [0] = 1f;
            }

            if (Projectile.ai [0] != 0.0) {
                Projectile.tileCollide = false;
                Vector2 vector2 = new Vector2(Projectile.position.X + (float) Projectile.width * 0.5f, Projectile.position.Y + (float) Projectile.height * 0.5f);
                float num4 = player.position.X + (float) (player.width / 2) - vector2.X;
                float num5 = player.position.Y + (float) (player.height / 2) - vector2.Y;
                float num6 = (float) Math.Sqrt(num4 * num4 + num5 * num5);
            }
            else {
                Vector2 zero = Vector2.Zero;
                Projectile.tileCollide = true;
                if (flag1 || flag2) {
                    int num4 = (int) (Projectile.position.X + (double) (Projectile.width / 2)) / 16;
                    int num5 = (int) (Projectile.position.Y + (double) (Projectile.height / 2)) / 16;
                    if (flag1)
                        --num4;
                    if (flag2)
                        ++num4;
                    if (WorldGen.SolidTile(num4 + (int) Projectile.velocity.X, num5))
                        flag4 = true;
                }
                if (player.position.Y + player.height - 8.0 > Projectile.position.Y + Projectile.height)
                    flag3 = true;
                Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
                if (!flag3 && (Projectile.velocity.X < 0 || Projectile.velocity.X > 0)) {
                    int num4 = (int) (Projectile.position.X + (double) (Projectile.width / 2)) / 16;
                    int num5 = (int) (Projectile.position.Y + (double) (Projectile.height / 2)) / 16 + 1;
                    if (flag1)
                        --num4;
                    if (flag2)
                        ++num4;
                    WorldGen.SolidTile(num4, num5);
                }
                if (flag4) {
                    int index1 = (int) (Projectile.position.X + (double) (Projectile.width / 2)) / 16;
                    int index2 = (int) (Projectile.position.Y + (double) Projectile.height) / 16 + 1;
                    if (WorldGen.SolidTile(index1, index2) || Main.tile [index1, index2].IsHalfBlock || Main.tile [index1, index2].Slope > 0) {
                        try {
                            DropRandomCoin();
                            int num4 = (int) (Projectile.position.X + (double) (Projectile.width / 2)) / 16;
                            int num5 = (int) (Projectile.position.Y + (double) (Projectile.height / 2)) / 16;
                            if (flag1)
                                --num4;
                            if (flag2)
                                ++num4;
                            int num6 = num4 + (int) Projectile.velocity.X;
                            if (!WorldGen.SolidTile(num6, num5 - 1) && !WorldGen.SolidTile(num6, num5 - 2))
                                Projectile.velocity.Y = -5.1f;
                            else if (!WorldGen.SolidTile(num6, num5 - 2))
                                Projectile.velocity.Y = -7.1f;
                            else if (WorldGen.SolidTile(num6, num5 - 5))
                                Projectile.velocity.Y = -11.1f;
                            else if (WorldGen.SolidTile(num6, num5 - 4))
                                Projectile.velocity.Y = -10.1f;
                            else
                                Projectile.velocity.Y = -9.1f;
                        }
                        catch {
                            Projectile.velocity.Y = -9.1f;
                        }
                    }
                }
            }
        }*/

        private void DropRandomCoin () {
            SoundEngine.PlaySound(SoundID.Coins, Projectile.Center);
            int coinType = Main.rand.Next(4);
            if (coinType == 0) Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.CopperCoin, 1);
            if (coinType == 1) {
                if (Main.rand.NextBool(10))
                    Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.SilverCoin, 1);
                else Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.CopperCoin, 1);
            }
            if (coinType == 2) {
                if (Main.rand.NextBool(50))
                    Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.GoldCoin, 1);
                else Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.CopperCoin, 1);
            }
            if (coinType == 3) {
                if (Main.rand.NextBool(150))
                    Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.PlatinumCoin, 1);
                else Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Center, ItemID.CopperCoin, 1);
            }
        }
    }
}