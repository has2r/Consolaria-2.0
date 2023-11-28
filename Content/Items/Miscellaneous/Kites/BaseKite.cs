using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Miscellaneous.Kites;

public abstract class BaseKiteItem : ModItem {
	public sealed override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
    }

    public sealed override void SetDefaults() {
        Item.width = 20;
        Item.height = 28;
        Item.DefaultToThrownWeapon(SetKiteProjectileType(), 30, 2f);
        Item.consumable = false;
        Item.noUseGraphic = true;
        Item.maxStack = 1;
        Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(gold: 2));
    }

    protected abstract int SetKiteProjectileType();

    public sealed override bool CanUseItem(Player player)
        => player.ownedProjectileCounts[SetKiteProjectileType()] < 1;

    public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        => true;

    public sealed override void ModifyTooltips(List<TooltipLine> tooltips) {
        TooltipLine tooltip = new(Mod, "Kite Tooltip", Language.GetTextValue("CommonItemTooltip.Kite").Replace("<right>", "Right Click"));
        tooltips.Add(tooltip);
    }
}
