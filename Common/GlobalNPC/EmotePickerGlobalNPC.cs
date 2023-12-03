using Consolaria.Content.EmoteBubbles;
using Consolaria.Common.ModSystems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Consolaria.Common.GlobalNPCs
{
	public class EmotePickerGlobalNPC : GlobalNPC
	{
		public override int? PickEmote(NPC npc, Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor) 
		{
			if (Main.rand.NextBool(2) && DownedBossSystem.downedLepus) 
			{
				emoteList.Add(ModContent.EmoteBubbleType<LepusEmote>());
			}

			if (Main.rand.NextBool(3) && DownedBossSystem.downedOcram) 
			{
				emoteList.Add(ModContent.EmoteBubbleType<OcramEmote>());
			}

			if (Main.rand.NextBool(4) && DownedBossSystem.downedTurkor) 
			{
				emoteList.Add(ModContent.EmoteBubbleType<TurkorEmote>());
			}
			
			return base.PickEmote(npc, closestPlayer, emoteList, otherAnchor);
		}
	}
}