using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Melee {
    public class GreatDrumstick : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Great Drumstick");
            Tooltip.SetDefault("Covers enemies in oil" + "\nSet oiled enemies on fire for extra damage" + "\n'I like large fries, but not fried turkey'");

            SacrificeTotal = 1;
        }

        public override void SetDefaults () {
            int width = 38; int height = 42;
            Item.Size = new Vector2(width, height);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 30;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 30;
            Item.knockBack = 8;

            Item.value = Item.buyPrice(gold: 1, silver: 20);
            Item.rare = ItemRarityID.Orange;

            Item.UseSound = SoundID.Item95;
        }

        public override void OnHitNPC (Player player, NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.Oiled, 600);

        public override void OnHitPvp (Player player, Player target, int damage, bool crit)
            => target.AddBuff(BuffID.Oiled, 600);
    }
}