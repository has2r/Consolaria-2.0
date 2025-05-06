using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.RoA.DruidWeapons;

sealed class Eggplant_Shoot : ModProjectile {
    private ref float ApplyPhysicsTimer => ref Projectile.ai[0];

    public override string Texture => Eggplant.Path + "_Shoot";

    public override void SetDefaults() {
        bool shouldChargeWreath = true;
        bool shouldApplyAttachedItemDamage = false;
        float wreathFillingFine = 0f;
        RoACompat.SetDruidicProjectileValues(Projectile, shouldChargeWreath, shouldApplyAttachedItemDamage, wreathFillingFine);

        Projectile.Size = 20 * Vector2.One;

        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.aiStyle = -1;
        Projectile.timeLeft = 240;

        Projectile.penetrate = 3;
    }

    public override void AI() {
        if (Main.windPhysics) {
            Projectile.velocity.X += Main.windSpeedCurrent * Main.windPhysicsStrength;
        }
        Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;
        if (ApplyPhysicsTimer >= 20f) {
            Projectile.velocity.Y += 0.3f;
            Projectile.velocity.X *= 0.98f;
            return;
        }
        ApplyPhysicsTimer += 1f;
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        SoundEngine.PlaySound(SoundID.NPCHit18, Projectile.position);

        return base.OnTileCollide(oldVelocity);
    }

    public override void OnKill(int timeLeft) {
        if (!Main.dedServ) {
            for (int i = 0; i < 3; i++)
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.oldVelocity * 0.2f, ModContent.Find<ModGore>("Consolaria/EggplantGore").Type, 1f);
        }
        for (int i = 0; i < 10; i++) {
            int dust = Dust.NewDust(Projectile.Center - Vector2.One * 10, 20, 20, DustID.Water_Desert, Projectile.velocity.X * 0.3f, 0, 0, new Color(250, 200, 100).MultiplyRGB(Color.Purple), 1.5f);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].scale *= 0.9f;
            int dust2 = Dust.NewDust(Projectile.position - Vector2.One * 10, 20, 20, DustID.Water_Desert, 0, 0, 0, new Color(250, 200, 100).MultiplyRGB(Color.Purple), 1.5f);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].scale *= 0.9f;
        }
    }
}
