using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs {
	public class GoldenCarrotDebuff : ModBuff {

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Golden Carrot Curse");
			Description.SetDefault("Cannot eat anymore golden carrots\nDefense is slightly decreased\n'I TOLD YOU THE CARROT WAS GOLD, I TOLD YOU!!!'");
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 4;
		}
	}
}