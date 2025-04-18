using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Drone : ConsolariaFlyingPet {
        public override int maxFrames => 2;
        public override int PreviewOffsetX => -11;
        public override int PreviewOffsetY => -20;
        public override int PreviewSpriteDirection => -1;
        public override bool isLightPet => false;

        public override void SetDefaults() {
            int width = 40; int height = 26;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Drone>()))
                Projectile.timeLeft = 2;

            FloatingAI(tilt: 0.05f);
            int frameTime = 2;
            Animation(frameTime);
        }
    }
}