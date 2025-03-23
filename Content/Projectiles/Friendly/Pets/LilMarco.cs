using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class LilMarco : ConsolariaFlyingPet {
        public override int maxFrames => 4;
        public override bool isLightPet => true;

        public override void SetDefaults() {
            int width = 44; int height = 40;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.LilMarco>()))
                Projectile.timeLeft = 2;

            FloatingAI(tilt: 0.05f);
            int frameTime = 5;
            Animation(frameTime);
            LightColor(Color.DarkViolet, 0.85f);
        }
    }
}