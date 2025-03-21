using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class AndroidGuy : ConsolariaPet {
        public override int maxFrames => 9;
        public override int PreviewFirstFrame => 1;
        public override int PreviewLastFrame => 5;
        public override int PreviewOffsetX => -3;
        public override int PreviewSpriteDirection => -1;

        public override void SetStaticDefaults() {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            int width = 32; int height = 42;
            Projectile.Size = new Vector2(width, height);

            DrawOffsetX -= 10;
            DrawOriginOffsetY = -6;

            base.SetDefaults();
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Content.Buffs.Android>()))
                Projectile.timeLeft = 2;

            WalkerAI();
            PassiveAnimation(idleFrame: 0, jumpFrame: 6);
            int finalFrame = maxFrames - 4;
            WalkingAnimation(walkingAnimationSpeed: 3, walkingFirstFrame: 1, finalFrame);
            FlyingAnimation(flyingAnimationSpeed: 3, flyingFirstFrame: 5, flyingLastFrame: 8);
            if (isFlying) {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, 16).RotatedBy(Projectile.rotation), 0, 0, DustID.Cloud, 0, 0, 50, default, 1.4f);
                Main.dust[dust].velocity = Vector2.Zero;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
            }
        }
    }
}