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
			dust.GetColor(ConsolePlayer.DiscoColor);
			if (!dust.noGravity) dust.velocity.Y += 0.05f;      
            if (!dust.noLight) 
				Lighting.AddLight(dust.position, ConsolePlayer.DiscoColor.R * 0.0025f, ConsolePlayer.DiscoColor.G * 0.0025f, ConsolePlayer.DiscoColor.B * 0.0025f);		
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
			=> ConsolePlayer.DiscoColor;
	}
}