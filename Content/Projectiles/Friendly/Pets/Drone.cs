using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Drone : ConsolariaFlyingPet {
        public override int maxFrames => 2;
        public override bool isLightPet => false;

        public override void SetDefaults () {
            int width = 70; int height = 40;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Drone>()))
                Projectile.timeLeft = 2;

            FloatingAI(tilt: 0.05f);
            int frameTime = 2;
            Animation(frameTime);
        }
    }
}