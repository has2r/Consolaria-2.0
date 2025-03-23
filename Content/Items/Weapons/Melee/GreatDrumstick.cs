using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Melee {
    public class GreatDrumstick : ModItem {
        public override void SetDefaults() {
            int width = 38; int height = 42;
            Item.Size = new Vector2(width, height);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 30;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 42;
            Item.knockBack = 8;

            Item.scale = 1.25f;

            Item.value = Item.buyPrice(gold: 1, silver: 20);
            Item.rare = ItemRarityID.Orange;

            Item.UseSound = SoundID.Item95;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
            => target.AddBuff(BuffID.Oiled, 600);

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
            => target.AddBuff(BuffID.Oiled, 600);
    }
}