using Microsoft.Xna.Framework;

using RoA.Core.Utility.Vanilla;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class ScytheFantasma : ThoriumItem_ScytheBase {
    public override void SetScytheDefaults() {
        Item.SetSizeValues(64, 60);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.Lime7, Item.sellPrice());
        Item.SetShootableValues<ScytheFantasma_Use>();

        Item.SetWeaponValues(64, 7f);
    }

    public sealed class ScytheFantasma_Use : ThoriumProjectile_ScytheBase {
        public override void SetScytheDefaults() {
            Projectile.SetSizeValues(164, 156);
        }

        public override void SetScytheValues(ref int dustCount, ref int dustType, ref Vector2 dustOffset) {
            dustCount = 5;
            dustType = DustID.PurpleTorch;
            dustOffset = new Vector2(0f, Main.rand.NextFloat(10f, 40f) - 10f);
        }

        public override void SafeModifyDamageHitbox(ref Rectangle hitbox) {
            hitbox.Inflate(12, 12);
        }

        public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) {
            bool shadowFlameDust = Main.rand.NextBool();
            dust.type = shadowFlameDust ? DustID.Shadowflame : DustID.PurpleTorch;
            dust.position += Main.rand.NextVector2Circular(7.5f, 7.5f);
            dust.scale = Main.rand.NextBool() ? 3f : 2f;
            dust.scale *= 0.5f;
            dust.alpha = 100;
            //dust.velocity += Vector2.UnitY.RotatedBy(MathHelper.WrapAngle(Projectile.rotation)) * Main.rand.NextFloat(0f, 0.5f) * 5f;
            dust.velocity += dust.position.DirectionTo(Projectile.Center) * Main.rand.NextFloat(0.75f, 1.25f) * 0.5f;
            dust.velocity += dust.position.DirectionTo(Projectile.Center) * Main.rand.NextFloat(0.75f, 1.25f) * Main.rand.NextFloatDirection();
            if (shadowFlameDust) {
                dust.scale *= 0.5f;
            }
            dust.noGravity = true;
            dust.noLight = dust.noLightEmittence = Main.rand.NextBool();
        }

        public override void OnFirstHit(NPC target, NPC.HitInfo hit, int damageDone) {
            if (!target.CanBeChasedBy()) {
                return;
            }

            TryToSpawnSpirit(target);
        }

        private void TryToSpawnSpirit(NPC target) {
            //SoundEngine.PlaySound(SoundID.NPCHit2, target.Center);

            if (!Projectile.IsOwnerLocal()) {
                return;
            }

            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), 
                                           target.Center,
                                           Vector2.One.RotatedByRandom(MathHelper.TwoPi) * 10f,
                                           ModContent.ProjectileType<ScytheFantasma_Spirit>(),
                                           Projectile.damage,
                                           Projectile.knockBack,
                                           Projectile.owner,
                                           target.whoAmI);
        }
    }

    public sealed class ScytheFantasma_Spirit : ThoriumProjectile_HealerBase {
        private static ushort TIMELEFT => Helper.SecondsToFrames(10);

        public int TargetWhoAmI => (int)Projectile.ai[0];
        public NPC Target => Main.npc[TargetWhoAmI];

        public bool CanExplode => Projectile.localAI[0] >= 30f || !Target.active;

        public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.75f).MultiplyAlpha(0.75f) * Projectile.Opacity;

        public override void SetStaticDefaults() {
            ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;

            ProjectileID.Sets.RocketsSkipDamageForPlayers[Type] = true;

            Projectile.SetFrameCount(4);
        }

        public override void PrepareBombToBlow() {
            Projectile.tileCollide = false;
            Projectile.alpha = 255;

            int size = 200;
            Projectile.Resize(size, size);
            Projectile.knockBack = 8f;
        }

        public override void SetHealerDefaults() {
            Projectile.SetSizeValues(20);

            Projectile.friendly = true;
            Projectile.penetrate = -1;

            Projectile.timeLeft = TIMELEFT;

            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.scale = 2f;
        }

        public override bool PreDraw(ref Color lightColor) {
            Color color = Projectile.GetAlpha(lightColor);
            Projectile.QuickDrawShadowTrails(color, 0.5f, 1);
            Projectile.QuickDrawAnimated(color);

            return false;
        }

        public override bool? CanDamage() => CanExplode ? base.CanDamage() : false;

        public override void AI() {
            Projectile.SetTrail(2, 5);

            Projectile.scale = Helper.Approach(Projectile.scale, 1f, 0.2f);

            if (Projectile.localAI[0] == 0f) {
                Projectile.localAI[0] = 1f;
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

                SoundEngine.PlaySound(SoundID.NPCDeath39 with { Pitch = 0.5f, Volume = 0.25f, PitchVariance = 0.1f }, Projectile.Center);

                for (int num561 = 0; num561 < 20; num561++) {
                    bool shadowFlameDust = Main.rand.NextBool();
                    int dustType = shadowFlameDust ? DustID.Shadowflame : DustID.PurpleTorch;
                    int num562 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100);
                    Main.dust[num562].position = Projectile.Center + Main.rand.NextVector2Circular(40f, 40f);
                    Main.dust[num562].noGravity = true;
                    Main.dust[num562].velocity += Projectile.velocity * 0.25f;
                    Main.dust[num562].fadeIn = 1f;
                    Main.dust[num562].velocity.X *= 2f;
                    Main.dust[num562].velocity.Y *= 2f;
                    Main.dust[num562].velocity.Y -= Main.rand.NextFloat() * 1.5f;
                }
            }

            {
                int num876 = Dust.NewDust(Projectile.position - Vector2.One * 1f, Projectile.width, Projectile.height, DustID.PurpleTorch);
                Dust dust = Main.dust[num876];
                dust.velocity *= 0.5f;
                dust.velocity += Vector2.UnitY.RotatedBy(Projectile.rotation - MathHelper.Pi * -Projectile.direction) * Main.rand.NextFloat(0f, 0.5f) * 5f;
                bool shadowFlameDust = Main.rand.NextBool();
                dust.type = shadowFlameDust ? DustID.Shadowflame : DustID.PurpleTorch;
                dust.scale = Main.rand.NextBool() ? 3f : 2f;
                dust.scale *= 0.5f;
                dust.alpha = 100;
                Main.dust[num876].noGravity = true;
            }

            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
                Projectile.PrepareBombToBlow();
            }

            if (!Target.active) {
                if (Projectile.owner == Main.myPlayer) {
                    Projectile.PrepareBombToBlow();
                }
                Projectile.Kill();
                return;
            }

            Projectile.localAI[0]++;
            Helper.InertiaMoveTowards(ref Projectile.velocity, Projectile.Center, Target.Center, 10f, Helper.Clamp01(Projectile.localAI[0] / 60f) * 10f);

            //Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.velocity.ToRotation() - MathHelper.PiOver2, 0.25f);
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.AngleTo(Target.Center) - MathHelper.PiOver2, 0.1f);

            Projectile.Animate(6);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
            //if (!target.CanBeChasedBy()) {
            //    return;
            //}

            if (target.whoAmI != Target.whoAmI) {
                return;
            }

            if (CanExplode) {
                if (Projectile.owner == Main.myPlayer) {
                    Projectile.PrepareBombToBlow();
                }
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft) {
            Explode();
        }

        private void Explode() {
            if (!CanExplode) {
                return;
            }

            int size = 200;
            Projectile.Resize(size, size);

            //SoundEngine.PlaySound(SoundID.NPCDeath39, Projectile.Center);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            // Smoke Dust spawn
            for (int i = 0; i < 50; i++) {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.position = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.5f;
                dust.velocity *= 1.4f;
            }

            // Fire Dust spawn
            for (int i = 0; i < 80; i++) {
                bool shadowFlameDust = Main.rand.NextBool();
                int dustType = shadowFlameDust ? DustID.Shadowflame : DustID.PurpleTorch;
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 3f);
                if (shadowFlameDust) {
                    dust.scale *= 0.5f;
                }
                dust.position = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.5f;
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
                dust.position = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.5f;
                dust.velocity *= 3f;
                if (shadowFlameDust) {
                    dust.scale *= 0.5f;
                }
            }

            // Large Smoke Gore spawn
            if (!Main.dedServ) {
                for (int g = 0; g < 2; g++) {
                    int goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2) - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2) - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2) - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2) - 24f, Projectile.position.Y + (Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                }
            }
        }
    }
}
