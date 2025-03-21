using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets;

public class SuspiciousLookingApple : PetItem {
    public override void SetStaticDefaults()
        => Item.ResearchUnlockCount = 1;

    public override void SetDefaults() {
        Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.Worm>(), ModContent.BuffType<Buffs.Worm>());

        int width = 22, height = 24;
        Item.Size = new Vector2(width, height);

        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 3);
    }
}