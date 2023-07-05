using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets; 

public class OldWalkingStick : PetItem  {
	public override void SetStaticDefaults () 
		=> Item.ResearchUnlockCount = 1;

	public override void SetDefaults () {
		Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.OldLady>(), ModContent.BuffType<Buffs.OldLady>());

		int width = 32, height = 28;
		Item.Size = new Vector2(width, height);

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.buyPrice(platinum: 1);
    }
}