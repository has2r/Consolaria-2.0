using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.Thorium.Dusts;

public sealed class JadeDust : JadeDust_Base { }
public sealed class JadeDust2 : JadeDust_Base { }

public abstract class JadeDust_Base : ModDust {
    public override Color? GetAlpha(Dust dust, Color lightColor) {
        lightColor = Color.Lerp(lightColor, Color.White, 0.8f);
        return new Color(lightColor.R, lightColor.G, lightColor.B, 25);
    }

    public override bool Update(Dust dust) {
        dust.BasicDust();

        if (dust.customData != null && dust.customData is Projectile projectile) {
            if (projectile.active) {
                Projectile player9 = (Projectile)dust.customData;
                dust.position += player9.position - player9.oldPosition;
            }
        }

        float num111 = dust.scale;
        if (num111 > 1f)
            num111 = 1f;

        if (!dust.noLight)
            Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num111 * 0.2f, num111 * 0.7f, num111 * 1f);

        if (dust.noGravity) {
            dust.velocity *= 0.93f;
            if (dust.fadeIn == 0f)
                dust.scale += 0.0025f;
        }

        dust.velocity *= new Vector2(0.97f, 0.99f);
        if (dust.customData != null && dust.customData is Player) {
            Player player9 = (Player)dust.customData;
            dust.position += player9.position - player9.oldPosition;
        }

        dust.scale -= 0.01f;

        return false;
    }
}
