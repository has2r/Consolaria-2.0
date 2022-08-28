using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Werewolf : ConsolariaPet {
        public override int maxFrames => 20;

        public override void SetDefaults () {
            int width = 34; int height = 52;
            Projectile.Size = new Vector2(width, height);

            base.SetDefaults();
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Werewolf>()))
                Projectile.timeLeft = 2;

            WalkerAI();
            WalkingAnimation(2, 2, 15);
            int finalFrame = maxFrames - 1;
            FlyingAnimation(4, 16, finalFrame);

            double rotation = (Math.PI / 2) * player.velocity.X * 0.08f;
            if (isFlying)
            Projectile.rotation = (float) rotation;
        }
    }
}