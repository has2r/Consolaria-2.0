using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs {
	public class Stunned : ModBuff {

		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Stunned");
			// Description.SetDefault("Can't move");
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex) 
			=> npc.GetGlobalNPC<StunnedNPC>().stunned = true;
	}

	internal class StunnedNPC : GlobalNPC  {
		public override bool InstancePerEntity => true;

		public bool stunned;
		private Rectangle NpcFrame;

		public override void ResetEffects(NPC npc)
			=> stunned = false;

		public override void SetDefaults(NPC npc) {
			if (!stunned)
				NpcFrame = npc.frame;
			else
				npc.frame = NpcFrame;
		}

		public override bool PreAI(NPC npc) {
			if (stunned) {
				npc.localAI[0] = 0;
				npc.localAI[1] = 0;
				npc.ai[0] = 0;
				npc.ai[1] = 0;

				npc.velocity = new Vector2(0, 6);
				npc.frameCounter = 0;
				npc.noGravity = false;
				return false;
			}
			return base.PreAI(npc);
		}

		public override void FindFrame(NPC npc, int frameHeight) {
			if (stunned)
				return;
		}
	}
}