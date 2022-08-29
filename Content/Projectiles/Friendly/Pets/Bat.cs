using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Bat : ConsolariaFlyingPet {
        public override int maxFrames => 4;
        public override bool isLightPet => false;

        public override void SetDefaults () {
            int width = 20; int height = 18;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Bat>()))
                Projectile.timeLeft = 2;

            bool turnRight = false; float tilt = 0.03f;
            FloatingAI(turnRight, tilt);
            int frameTime = 2;
            Animation(frameTime);
        }
    }
}