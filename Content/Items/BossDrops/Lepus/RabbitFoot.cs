using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.BossDrops.Lepus {
	public class RabbitFoot : ModItem {
		public override void SetStaticDefaults () {
			Tooltip.SetDefault("Summons a baby Lepus" + "\n'Provides an illusion of good luck'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
		}

		public override void SetDefaults () {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.SmallLepus>(), ModContent.BuffType<Buffs.SmallLepus>());

			int width = 20; int height = 32;
			Item.Size = new Vector2(width, height);

			Item.master = true;
			Item.value = Item.sellPrice(gold: 5);
		}

		public override void UseStyle (Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600);
		}
	}
}