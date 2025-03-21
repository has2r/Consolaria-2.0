using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class MythicalWyvernling : ConsolariaFlyingPet {
        public override int maxFrames => 5;
        public override bool isLightPet => true;

        public override void SetDefaults () {
            int width = 60; int height = 50;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.MythicalWyvernling>()))
                Projectile.timeLeft = 2;

            FloatingAI(tilt: 0.025f);
            int frameTime = 8;
            Animation(frameTime);
            LightColor(new Color(255, 140, 0), 0.7f);
        }
    }
}