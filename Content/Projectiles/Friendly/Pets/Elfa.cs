using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Elfa : ConsolariaPet {
        public override int maxFrames => 9;
        public override int PreviewFirstFrame => 1;
		public override int PreviewLastFrame => 8;
		public override int PreviewSpriteDirection => -1;

        public override void SetStaticDefaults () {
            ProjectileID.Sets.TrailCacheLength [Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode [Projectile.type] = 0;

            base.SetStaticDefaults();
        }

        public override void SetDefaults () {
            int width = 30; int height = 50;
            Projectile.Size = new Vector2(width, height);

            DrawOffsetX -= 10;

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Elfa>()))
                Projectile.timeLeft = 2;

            WalkerAI();
            PassiveAnimation(idleFrame: 0, jumpFrame: 3);
            int finalFrame = maxFrames - 1;
            WalkingAnimation(walkingAnimationSpeed: 3, walkingFirstFrame: 1, finalFrame);
            FlyingAnimation(oneFrame: true);

            if (isFlying) {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float) Math.PI / 2;
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, 16).RotatedBy(Projectile.rotation), 0, 0, DustID.FireworkFountain_Red, 0, 0, 50, default, 1.4f);
                Main.dust [dust].velocity = Vector2.Zero;
                Main.dust [dust].noGravity = true;
                Main.dust [dust].noLight = true;
            }
        }
    }
}