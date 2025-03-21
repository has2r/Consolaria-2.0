using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs {
    public class GoldenCarrotDebuff : ModBuff {

		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 4;
		}
	}
}