using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Dusts;

public sealed class OmunikodoDust : ModDust {
    public override Color? GetAlpha(Dust dust, Color lightColor) {
        lightColor = Color.Lerp(lightColor, Color.White, 0.8f);
        return new Color(lightColor.R, lightColor.G, lightColor.B).MultiplyRGB(dust.color) with { A = (byte)dust.alpha };
    }

    public override void OnSpawn(Dust dust) {
        dust.velocity.Y = (float)Main.rand.Next(-10, 6) * 0.1f;
        dust.velocity.X *= 0.3f;
        dust.scale *= 0.7f;
    }

    public override bool Update(Dust dust) {
        dust.BasicDust();

        if (dust.customData != null && dust.customData is int) {
            if ((int)dust.customData == 0) {
                if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f) {
                    dust.scale *= 0.9f;
                    dust.velocity *= 0.25f;
                }
            }
            else if ((int)dust.customData == 1) {
                dust.scale *= 0.98f;
                dust.velocity.Y *= 0.98f;
                if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f) {
                    dust.scale *= 0.9f;
                    dust.velocity *= 0.25f;
                }
            }
        }

        float num111 = dust.scale;
        if (num111 > 1f)
            num111 = 1f;
        Vector3 color = dust.color.ToVector3() * 0.625f;
        Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num111 * color.X, num111 * color.Y, num111 * color.Z);

        //if (!dust.noLight && !dust.noLightEmittence) {
        //    float num56 = dust.scale * 1.4f;
        //    if (num56 > 0.6f)
        //        num56 = 0.6f;

        //    Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num56, num56 * 0.65f, num56 * 0.4f);
        //}

        return false;
    }
}
