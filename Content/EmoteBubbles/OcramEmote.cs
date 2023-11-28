using Consolaria.Common.ModSystems;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Consolaria.Content.EmoteBubbles
{
	public class OcramEmote : ModEmoteBubble
	{
		public override void SetStaticDefaults() 
		{
			AddToCategory(EmoteID.Category.Dangers);
		}

		public override bool IsUnlocked() 
		{
			return DownedBossSystem.downedOcram;
		}
	}
}