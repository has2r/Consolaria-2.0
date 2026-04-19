using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets;

public sealed class BleachCanister : PetItem {
    public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

    public override void SetDefaults() {
        Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.AlbinoRunt>(), ModContent.BuffType<Buffs.AlbinoRuntBuff>());

        Item.SetSizeValues(20, 30);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.Orange3, Item.sellPrice(gold: 3));
    }
}