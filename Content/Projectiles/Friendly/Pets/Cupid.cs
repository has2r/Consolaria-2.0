using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Cupid : ConsolariaFlyingPet {
        public override int maxFrames => 4;
        public override bool isLightPet => true;

        public override void SetDefaults () {
            int width = 38; int height = 50;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Cupid>()))
                Projectile.timeLeft = 2;

            bool turnRight = false; float tilt = 0.025f;
            FloatingAI(turnRight, tilt);
            int frameTime = 8;
            Animation(frameTime);
            LightColor(new Color(255, 105, 180));
        }
    }
}