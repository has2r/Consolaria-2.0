using Consolaria.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Dusts
{
	public class RomanFlame : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noLight = false;
            dust.velocity.Y = Main.rand.Next(-10, 11) * 0.15f;
            dust.velocity.X *= 0.25f;
            dust.scale *= 0.75f;
		}

        public override bool MidUpdate(Dust dust) {
			dust.GetColor(UtilsPlayer.DiscoColor);
			if (!dust.noGravity) dust.velocity.Y += 0.05f;      
            if (!dust.noLight) 
				Lighting.AddLight(dust.position, UtilsPlayer.DiscoColor.R * 0.0025f, UtilsPlayer.DiscoColor.G * 0.0025f, UtilsPlayer.DiscoColor.B * 0.0025f);		
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
			=> UtilsPlayer.DiscoColor;
	}
}