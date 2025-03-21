using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Zombie : ConsolariaPet {
        public override int maxFrames => 7;
        public override int PreviewLastFrame => 3;
        public override int PreviewOffsetX => 0;
		public override int PreviewOffsetY => 0;
		public override int PreviewSpriteDirection => -1;

        public override void SetDefaults () {
            int width = 26; int height = 50;
            Projectile.Size = new Vector2(width, height);

            DrawOffsetX -= 20;

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Zombie>()))
                Projectile.timeLeft = 2;

            WalkerAI();
            PassiveAnimation(idleFrame: 0, jumpFrame: 2);
            WalkingAnimation(walkingAnimationSpeed: 3, walkingFirstFrame: 0, walkingLastFrame: 2);
            int finalFrame = maxFrames - 1;
            FlyingAnimation(flyingAnimationSpeed: 4, flyingFirstFrame: 3, finalFrame);
        }
    }
}