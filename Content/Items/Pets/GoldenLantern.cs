using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets {
	public class GoldenLantern : PetItem {
		public override void SetStaticDefaults () {
			Tooltip.SetDefault("Summons a mythical wyvernling to provide light");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
		}

		public override void SetDefaults () {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.MythicalWyvernling>(), ModContent.BuffType<Buffs.MythicalWyvernling>());

			int width = 28; int height = 40;
			Item.Size = new Vector2(width, height);

			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(gold: 10);
		}
	}
}