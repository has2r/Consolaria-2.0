using Consolaria.Content.Projectiles.Enemies;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RoA.Core.Utility.Vanilla;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class SpineCracker : ThoriumItem_ThrowerBase {
    public override void SetThrowerDefaults() {
        Item.SetSizeValues(40, 36);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.White0, Item.sellPrice());
        Item.SetShootableValues<SpineCracker_Throw>(12f);

        Item.SetWeaponValues(60, 5f);
        Item.SetDefaultsToUsable(ItemUseStyleID.Swing, 36, showItemOnUse: false, autoReuse: true, useSound: SoundID.Item19);
    }

    public override void SetThrowerValues(ref bool isThrowerNon, ref bool IsThrowerNeedle, ref bool IsThrowerTomahawk, ref bool IsThrowerCaltrop) {
        isThrowerNon = true;
    }

    public sealed class SpineCracker_Throw : ThoriumProjectile_TomahawkBase {
        public override string Texture => Helper.GetItemTexturePath<SpineCracker>();

        public override float VelocityMultiplier => 0.975f;
        public override int Penetration => -1;
        public override int TimeLeftToRotateSlow => 3 * base.TimeLeftToRotateSlow;
        public override float RotationSpeedSlow => 0.15f * Helper.Clamp01(Projectile.timeLeft / (float)base.TimeLeftToRotateSlow);
        public override float RotationSpeedFast => 0.15f;

        public ref float ScytheSpawnTimer => ref Projectile.localAI[0];

        public override void SafeSetStaticDefaults() {
            Projectile.SetTrail(2, 8);
        }

        public override void SafeSetDefaults() {
            Projectile.SetSizeValues(36, 36);
        }

        public override void SafeAI() {
            if (Main.rand.NextBool(2) && Main.rand.NextChance(Helper.Clamp01(Projectile.timeLeft / (float)base.TimeLeftToRotateSlow) / 2f)) {
                int num876 = Dust.NewDust(Projectile.position - Vector2.One * 1f, Projectile.width, Projectile.height, DustID.PurpleTorch);
                Dust dust = Main.dust[num876];
                dust.velocity *= 2f;
                dust.velocity += Vector2.UnitY
                    .RotatedBy(Projectile.rotation - MathHelper.Pi * -Projectile.direction - MathHelper.PiOver4 * 1.25f * -Projectile.direction)
                    .RotatedBy(MathHelper.PiOver2 * -Projectile.direction) * 2.5f;
                bool shadowFlameDust = Main.rand.NextBool();
                dust.type = shadowFlameDust ? DustID.Shadowflame : DustID.PurpleTorch;
                dust.scale = Main.rand.NextBool() ? 3f : 2f;
                dust.scale *= 0.5f;
                dust.alpha = 100;
                dust.position += dust.position.DirectionFrom(Projectile.Center) * Projectile.width * 0.25f;
                Main.dust[num876].noGravity = true;
            }

            if (Projectile.Opacity >= 1f && Projectile.timeLeft < base.TimeLeftToRotateSlow * 2 && ++ScytheSpawnTimer > 10f) {
                ScytheSpawnTimer = 0f;

                if (Projectile.IsOwnerLocal()) {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(),
                                                   Projectile.Center,
                                                   Vector2.UnitY
                                                          .RotatedBy(Projectile.rotation - MathHelper.Pi * -Projectile.direction - MathHelper.PiOver4 * 1.25f * -Projectile.direction)
                                                          .RotatedBy(MathHelper.PiOver2 * -Projectile.direction) * 5f,
                                                   ModContent.ProjectileType<SpineCracker_Scythe>(),
                                                   Projectile.damage,
                                                   Projectile.knockBack,
                                                   Projectile.owner);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Projectile.QuickDrawShadowTrails(lightColor * Projectile.Opacity, 0.5f, 1, 0f);
            Projectile.QuickDraw(lightColor * Projectile.Opacity);

            return false;
        }

        public override void OnKill(int timeLeft) {
            
        }
    }

    public sealed class SpineCracker_Scythe : ThoriumProjectile_ThrowerBase {
        private float rotationTimer = (float)Math.PI;

        public override string Texture => Helper.GetProjectileTexturePath<OcramScythe>();

        public override void SetStaticDefaults() {
            Projectile.SetTrail(2, 8);
        }

        public override void SetThrowerDefaults() {
            int width = 60; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;

            Projectile.penetrate = 1;
            Projectile.tileCollide = false;

            Projectile.light = 0.25f;
            Projectile.scale = 0.6f;
        }

        public override void AI() {
            while (Projectile.scale < 1)
                Projectile.scale += 0.1f;

            if (Projectile.timeLeft > 200)
                Projectile.velocity *= 1.05f;

            int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
            Main.dust[dust2].noGravity = true;

            Projectile.rotation += 2 / rotationTimer;
            rotationTimer += 0.05f;
        }

        public override void OnKill(int timeLeft) {
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 30; i++) {
                    int num506 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.5f);
                    Main.dust[num506].noGravity = true;
                    Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.1f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/OcramScythe_Glow");
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            SpriteEffects effects = (Projectile.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length - 1; k++) {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2f;
                Color color = new Color(50 - k * 6, 10, 110 + k * 5, 0) * (8 - k) * 0.125f;
                spriteBatch.Draw(texture, drawPos, null, color, Projectile.oldRot[k], drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length, effects, 0f);
                spriteBatch.Draw(texture, drawPos - Projectile.oldPos[k] * 0.5f + Projectile.oldPos[k + 1] * 0.5f, null, color, Projectile.oldRot[k] * 0.5f + Projectile.oldRot[k + 1] * 0.5f, drawOrigin, Projectile.scale - (k + 0.5f) / (float)Projectile.oldPos.Length, effects, 0f);
            }
            spriteBatch.Draw(Projectile.GetTexture(), Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
            => target.AddBuff(BuffID.ShadowFlame, 180);

        public override Color? GetAlpha(Color lightColor)
            => new Color(255, 255, 255, 200);
    }
}
