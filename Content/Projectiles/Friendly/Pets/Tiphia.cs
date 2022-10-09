using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Tiphia : ConsolariaFlyingPet {
        public override int maxFrames => 3;
        public override bool isLightPet => false;

        public override void SetDefaults () {
            int width = 34; int height = 34;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Tiphia>()))
                Projectile.timeLeft = 2;

            FloatingAI(tilt: 0.025f);
            int frameTime = 6;
            Animation(frameTime);
        }
    }
}