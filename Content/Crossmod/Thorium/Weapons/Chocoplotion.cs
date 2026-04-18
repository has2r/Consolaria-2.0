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
    public override string Texture => "Consolaria/Content/Crossmod/Thorium/Weapons/Chocoplotion_Item";

    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

    public override void SetThrowerDefaults() {
        Item.SetSizeValues(30, 42);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.Blue1, Item.sellPrice());
        Item.SetShootableValues<Chocoplotion_Throw>(6f);

        Item.SetWeaponValues(12, 5f);
        Item.SetDefaultsToUsable(ItemUseStyleID.Swing, 40, showItemOnUse: false, autoReuse: true, useSound: SoundID.Item19);
    }

    public override void SetThrowerValues(ref bool isThrowerNon, ref bool IsThrowerNeedle, ref bool IsThrowerTomahawk, ref bool IsThrowerCaltrop) {
        isThrowerNon = true;
    }

    public sealed class Chocoplotion_Throw : ThoriumProjectile_ThrowerBase {
        private static ushort TIMELEFT => 180;

        public override string Texture => "Consolaria/Content/Crossmod/Thorium/Weapons/Chocoplotion";

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
            Vector2 position = Projectile.position;
            Projectile.position.Y -= 4f;
            Projectile.QuickDraw(lightColor * Projectile.Opacity);
            Projectile.position = position;

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
                    dust.position = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation - MathHelper.Pi + 0.4f * Projectile.spriteDirection) * 30f;

                    dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
                    dust.scale = 1f + Main.rand.Next(5) * 0.1f;
                    dust.noGravity = true;
                    dust.position = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation - MathHelper.Pi + 0.4f * Projectile.spriteDirection) * 30f;
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
            hitboxCenterFrac.Y += 0.1f;

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
            for (int i = 0; i < 20; i++) {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ChocoplotionDust>(), 0f, 0f, 0, default(Color), 1.15f + Main.rand.NextFloat(-0.1f, 0.1f));
                Main.dust[dustIndex].position = Projectile.Center + Main.rand.NextVector2Circular(30, 30) * 0.75f;
                Main.dust[dustIndex].velocity *= 1f;
                Main.dust[dustIndex].noGravity = true;
            }

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

            // Resize the projectile again so the explosion dust and gore spawn from the middle.
            // Rocket I: 22, Rocket III: 80, Mini Nuke Rocket: 50
            Projectile.Resize(22, 22);

            // Spawn a bunch of smoke dusts.
            for (int i = 0; i < 30; i++) {
                var smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
                smoke.velocity *= 1.4f;
            }

            // Spawn a bunch of fire dusts.
            for (int j = 0; j < 20; j++) {
                var fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3.5f);
                fireDust.noGravity = true;
                fireDust.velocity *= 7f;
                fireDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                fireDust.velocity *= 3f;
            }

            // Spawn a bunch of smoke gores.
            for (int k = 0; k < 2; k++) {
                float speedMulti = 0.4f;
                if (k == 1) {
                    speedMulti = 0.8f;
                }

                var smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
                smokeGore.velocity *= speedMulti;
                smokeGore.velocity += Vector2.One;
                smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
                smokeGore.velocity *= speedMulti;
                smokeGore.velocity.X -= 1f;
                smokeGore.velocity.Y += 1f;
                smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
                smokeGore.velocity *= speedMulti;
                smokeGore.velocity.X += 1f;
                smokeGore.velocity.Y -= 1f;
                smokeGore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, default, Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1));
                smokeGore.velocity *= speedMulti;
                smokeGore.velocity -= Vector2.One;
            }
        }
    }

    public sealed class Chocoplotion_Droplet : ThoriumProjectile_ThrowerBase {
        private static ushort TIMELEFT => Helper.SecondsToFrames(5);

        public override void SetThrowerDefaults() {
            Projectile.SetSizeValues(24);

            Projectile.friendly = true;
            Projectile.penetrate = 1;
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
            for (int i = 0; i < 10; i++) {
                int size = 8;
                int ind4 = Dust.NewDust(Projectile.Center - Vector2.One * size, size * 2, size * 2, ModContent.DustType<ChocoplotionDust>(), 0f, 0f, 0, default, 1.15f + Main.rand.NextFloat(-0.1f, 0.1f));
                Main.dust[ind4].velocity *= 0.5f;
                Main.dust[ind4].noGravity = true;
                Main.dust[ind4].velocity += Projectile.velocity * Main.rand.NextFloat() * 0.75f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            SoundEngine.PlaySound(new SoundStyle($"Terraria/Sounds/Drip_", 0, 2) { Pitch = -1f, Volume = 0.5f, PitchVariance = 0.6f }, Projectile.Center);

            Projectile.Kill();

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
                if (Projectile.velocity.Y < 0f) {
                    Projectile.velocity.Y += 0.2f;
                }
            }
            Projectile.velocity.X *= 0.9f;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.velocity.ToRotation() + MathHelper.PiOver2, 0.2f);

            if (Main.rand.NextBool(30) && Main.rand.NextChance(MathF.Max(0.25f, 1f - Helper.Clamp01(Projectile.ai[0] / 120f)))) {
                int size = 8;
                int ind4 = Dust.NewDust(Projectile.Center - Vector2.One * size, size * 2, size * 2, ModContent.DustType<ChocoplotionDust>(), 0f, 0f, 0, default, 1.15f + Main.rand.NextFloat(-0.1f, 0.1f));
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
