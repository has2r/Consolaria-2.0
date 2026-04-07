using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Dusts;

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
sealed class ChocoplotionDust : ModDust {
    public override bool Update(Dust dust) {
        dust.BasicDust();

        if (dust.velocity.Y < 0f) {
            dust.velocity.Y += 0.1f;
        }

        dust.velocity.Y += 0.1f;
        dust.velocity.X *= 0.9f;

        if (Collision.SolidCollision(dust.position, 4, 4)) {
            dust.velocity *= 0.5f;
        }

        return false;
    }
}
