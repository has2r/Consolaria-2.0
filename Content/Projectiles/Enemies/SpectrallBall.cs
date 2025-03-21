using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies {
    public class SpectrallBall : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetDefaults() {
            int width = 14; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = false;
            Projectile.hostile = true;

            Projectile.ignoreWater = true;
            Projectile.aiStyle = 6;

            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;

            Projectile.timeLeft = 180;
            Projectile.light = 0.05f;

            Projectile.extraUpdates = 5;
        }

        public override void AI() {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + (float)Math.PI;
            if (Main.netMode != NetmodeID.Server) {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}