using Consolaria.Content.Crossmod.Thorium.Dusts;

using Microsoft.Xna.Framework;

using RoA.Core.Utility.Vanilla;

using System;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class Chocoplotion : ThoriumItem_ThrowerBase {
    public override void SetThrowerDefaults() {
        Item.SetSizeValues(38, 50);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.White0, Item.sellPrice());
        Item.SetShootableValues<Chocoplotion_Throw>(6f);

        Item.SetWeaponValues(60, 5f);
        Item.SetDefaultsToUsable(ItemUseStyleID.Swing, 36, showItemOnUse: false, autoReuse: true, useSound: SoundID.Item19);
    }

    public override void SetThrowerValues(ref bool isThrowerNon, ref bool IsThrowerNeedle, ref bool IsThrowerTomahawk, ref bool IsThrowerCaltrop) {
        isThrowerNon = true;
    }

    public sealed class Chocoplotion_Throw : ThoriumProjectile_ThrowerBase {
        private static ushort TIMELEFT => 180;

        public override string Texture => Helper.GetItemTexturePath<Chocoplotion>();

        public override void SetStaticDefaults() {
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;

            ProjectileID.Sets.Explosive[Type] = true;

            ProjectileID.Sets.RocketsSkipDamageForPlayers[Type] = true;
        }

        public override void SetThrowerDefaults() {
            Projectile.SetSizeValues(50);

            Projectile.penetrate = -1;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.tileCollide = true;
            Projectile.timeLeft = TIMELEFT;
        }

        public override bool PreDraw(ref Color lightColor) {
            Projectile.QuickDraw(lightColor * Projectile.Opacity);

            return false;
        }

        public override void PrepareBombToBlow() {
            Projectile.alpha = 255;
            Projectile.position = Projectile.Center;
            Projectile.Center = Projectile.position;
            Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the grenade explodes on slopes.
            Projectile.alpha = 255; // Make the grenade invisible.

            // Resize the hitbox of the projectile for the blast "radius".
            // Rocket I: 128, Rocket III: 200, Mini Nuke Rocket: 250
            // Measurements are in pixels, so 128 / 16 = 8 tiles.
            Projectile.Resize(128, 128);
            // Set the knockback of the blast.
            // Rocket I: 8f, Rocket III: 10f, Mini Nuke Rocket: 12f
            Projectile.knockBack = 8f;
        }

        public override void AI() {
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
                Projectile.PrepareBombToBlow();
            }
            else {
                NPC target = ThoriumUtils.FindNearestNPC(Projectile, 600f);
                if (Projectile.ai[2] == 1f) {
					if (target is not null) {
						if (++Projectile.ai[1] > 60f) {
							Projectile.velocity.Y -= 3f;
							Projectile.velocity.X += Projectile.DirectionTo(target.Center).X * 5f;
							Projectile.ai[1] = 0f;
						}
					}
					else {
						Projectile.velocity.Y -= Math.Abs(Projectile.velocity.X) * 0.6f;
						Projectile.velocity.X *= 0.8f;
					}
                }

                //Projectile.tileCollide = Projectile.timeLeft < TIMELEFT - 5;

                // Smoke and fuse dust spawn. The position is calculated to spawn the dust directly on the fuse.
                if (Main.rand.NextBool()) {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
                    dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
                    dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
                    dust.noGravity = true;
                    dust.position = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation - MathHelper.Pi + 0.5f * Projectile.spriteDirection) * 26f;

                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
                    dust.scale = 1f + Main.rand.Next(5) * 0.1f;
                    dust.noGravity = true;
                    dust.position = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation - MathHelper.Pi + 0.5f * Projectile.spriteDirection) * 26f;
                }
            }

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 10f && Projectile.tileCollide)
                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f; // 0.1f for arrow gravity, 0.4f for knife gravity
            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;

            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.velocity.X * 0.1f, 0.2f);

            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0f).ToDirectionInt();

            Projectile.ai[2] = 0f;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = height = (int)(50 * 1f) - 2;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.ai[2] = 1f;

            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            if (Projectile.timeLeft > 4)
                Projectile.timeLeft = 4;
        }

        public override void OnKill(int timeLeft) {
            if (Projectile.IsOwnerLocal()) {
                int count = 6;
                for (int i = 0; i < count; i++) {
                    float angle = MathHelper.TwoPi * i / count;
                    float bulletSpeed = 4f;
                    bulletSpeed *= Main.rand.NextFloat(0.75f, 1.25f);
                    Vector2 velocity = Vector2.One.RotatedBy(angle + MathHelper.PiOver4 * 0.5f * Main.rand.NextFloatDirection()) * bulletSpeed;
                    Projectile.NewProjectileDirect(Projectile.GetSource_Death(),
                                                   Projectile.Center,
                                                   velocity,
                                                   ModContent.ProjectileType<Chocoplotion_Droplet>(),
                                                   Projectile.damage,
                                                   0f,
                                                   Projectile.owner);
                }
            }

            Projectile.Resize(150, 150);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            for (int i = 0; i < 15; i++) {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), 4, 4, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[dustIndex].velocity *= 1.2f;
            }

            for (int i = 0; i < 10; i++) {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), 4, 4, 6, 0f, 0f, 100, default(Color), 1f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 4f;
                dustIndex = Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), 4, 4, 6, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[dustIndex].velocity *= 2.5f;
            }

            if (!Main.dedServ) {
                for (int g = 0; g < 1; g++) {
                    int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), default(Vector2), Main.rand.Next(61, 64), 1f);
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), default(Vector2), Main.rand.Next(61, 64), 1f);
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y + (Projectile.height / 2)), default(Vector2), Main.rand.Next(61, 64), 1f);
                }
            }

            Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
        }
    }

    public sealed class Chocoplotion_Droplet : ThoriumProjectile_ThrowerBase {
        private static ushort TIMELEFT => Helper.SecondsToFrames(5);

        public override void SetThrowerDefaults() {
            Projectile.SetSizeValues(24);

            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.tileCollide = true;

            Projectile.timeLeft = TIMELEFT;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            width = height = (int)(24 * 1f) - 2;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnKill(int timeLeft) {
            for (int i = 0; i < 6; i++) {
                int ind4 = Dust.NewDust(Projectile.Center - Vector2.One * 4, 8, 8, ModContent.DustType<ChocoplotionDust>(), 0f, 0f, 0, default, 1.15f + Main.rand.NextFloat(-0.1f, 0.1f));
                Main.dust[ind4].velocity *= 0.5f;
                Main.dust[ind4].noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            return false;
        }

        public override void AI() {
            //Projectile.Opacity = Utils.GetLerpValue(0, 50, Projectile.timeLeft, true);

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 15f) {
                if (Projectile.velocity.Y == 0f) {
                    Projectile.velocity.X *= 0.95f;
                }
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
			
			if (Main.rand.NextBool(10) && Projectile.ai[0] < 50f) {
				int ind4 = Dust.NewDust(Projectile.Center - Vector2.One * 4, 8, 8, ModContent.DustType<ChocoplotionDust>(), 0f, 0f, 0, default, 1.15f + Main.rand.NextFloat(-0.1f, 0.1f));
                Main.dust[ind4].velocity *= 0.5f;
                Main.dust[ind4].noGravity = true;
			}
        }

        public override bool PreDraw(ref Color lightColor) {
            Projectile.QuickDrawShadowTrails(lightColor * Projectile.Opacity, 0.5f, 1, Projectile.rotation);
            Projectile.QuickDrawAnimated(lightColor * Projectile.Opacity, 0f);

            return false;
        }
    }
}
