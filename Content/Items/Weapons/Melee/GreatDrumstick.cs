using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Melee
{
    public class GreatDrumstick : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Great Drumstick");
            Tooltip.SetDefault("Covers enemies in oil and can set them on fire" + "\nOiled enemies take more damage from fire" + "\n'I like large fries, but not fried turkey'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 38; int height = 42;
            Item.Size = new Vector2(width, height);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 30;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 30;
            Item.knockBack = 8;
            Item.crit = 5;

            Item.value = Item.buyPrice(gold: 1, silver: 20);
            Item.rare = ItemRarityID.Orange;

            Item.UseSound = SoundID.Item95;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) {
            int buffType = BuffID.Oiled;
            target.AddBuff(buffType, 600);
            if (target.HasBuff(buffType) && Main.rand.Next(4) == 0)
                target.AddBuff(BuffID.OnFire, 300);          
        }

        public override void OnHitPvp(Player player, Player target, int damage, bool crit) {
            int buffType = BuffID.Oiled;
            target.AddBuff(buffType, 600);
            if (target.HasBuff(buffType) && Main.rand.Next(4) == 0)
                target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
