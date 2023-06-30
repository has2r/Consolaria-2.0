using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Friendly.McMoneypants;

public class MoneyDust : ModDust {
    public override Color? GetAlpha(Dust dust, Color lightColor)
        => lightColor;

    public override void OnSpawn(Dust dust) {
		int width = 12, height = 16;
		dust.frame = new Rectangle(0, 0, width, height);

        dust.noGravity = true;
        dust.noLight = false;

        dust.fadeIn = Main.rand.NextFloat(MathHelper.TwoPi);

        dust.scale = Main.rand.NextFloat(1.5f, 2.25f) * Main.rand.NextFloat(0.5f, 1f);
        dust.rotation = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
    }

	public override bool Update(Dust dust) {
		dust.position.Y += dust.velocity.Y;
		dust.velocity.Y += 0.03f;
		dust.scale *= 0.965f;
		dust.color *= 0.9f;
		if (dust.scale <= 0.35) {
			dust.active = false;
		}
		return false;
	}
}