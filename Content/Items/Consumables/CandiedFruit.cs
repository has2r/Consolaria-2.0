using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Consumables;

public class CandiedFruit : ModItem {
	public override void SetStaticDefaults() {
		ItemID.Sets.IsFood[Type] = true;
		ItemID.Sets.FoodParticleColors[Type] = new Color[3] {
			new Color(216, 75, 33),
			new Color(134, 0, 46),
			new Color(192, 49, 49)
		};

		Item.ResearchUnlockCount = 5;

		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
	}

	public override void SetDefaults() {
        Item.DefaultToFood(22, 22, 206, 3600 * 3);
        Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(silver: 1));
    }


    public sealed override void ModifyTooltips(List<TooltipLine> tooltips) {
        TooltipLine tooltip = new(Mod, "Food Tooltip", Language.GetTextValue("CommonItemTooltip.MediumStats"));
        tooltips.Insert(2, tooltip);
    }
}
