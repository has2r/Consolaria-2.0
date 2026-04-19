using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Dusts;

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public sealed class ViperDust : ModDust {
    public override void OnSpawn(Dust dust) {
        dust.velocity.X *= 0.5f;
        dust.velocity.Y = MathF.Abs(dust.velocity.Y);
    }

    public override bool Update(Dust dust) {
        dust.BasicDust();
        dust.velocity.Y += 0.1f;

        if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10)) {
            dust.velocity *= 0.5f;
        }

        return false;
    }
}
