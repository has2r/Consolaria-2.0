using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Dusts {
    public class Lepus : ModDust {
        public override void OnSpawn(Dust dust) {
            UpdateType = DustID.Smoke;

            dust.noGravity = true;
            dust.noLight = true;
            dust.rotation = Main.rand.NextFloat() * (float)Math.PI * 2f;
        }

        public override bool Update(Dust dust) {
            if (dust.alpha < 255) {
                dust.alpha += 10;
            }
            dust.rotation += dust.velocity.Y * 0.05f;
            dust.scale *= 0.99f;
            dust.velocity *= 0.99f;
            return true;
        }
    }
}