using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace Consolaria.Content.Dusts
{
	public class EggDust : ModDust
	{
		private bool succ = false;
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 16, 18);
			dust.noGravity = true;
			dust.noLight = true;
			dust.rotation = Main.rand.NextFloat() * (float)Math.PI * 2f;
			if (dust.alpha == 1) succ = true;
		}

		public override bool Update(Dust dust)
		{
			dust.velocity *= 0.99f;
			dust.position += dust.velocity;
			dust.rotation += succ ? dust.velocity.X * 0.1f : dust.velocity.Y * 0.05f;
			dust.scale *= succ ? 0.96f : 0.99f;
			if (dust.scale <= 0.8f)
			{
				dust.alpha += 5;
				if (dust.alpha >= 250) dust.active = false;
			}
			return false;
		}
	}
}