using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged {
    [AutoloadEquip(EquipType.Body)]
    public class TitanMail : ModItem {
        public static Lazy<Asset<Texture2D>> mailGlowmask;
        public override void Unload () => mailGlowmask = null;

        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Titan Mail");
            Tooltip.SetDefault("5% increased ranged damage" + "\n15 % increased ranged critical strike chance" + "\n20% chance to not consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;

            if (!Main.dedServ) {
                mailGlowmask = new(() => ModContent.Request<Texture2D>(Texture + "_Glow"));
                BodyGlowmask.RegisterData(Item.bodySlot, () => new Color(255, 255, 255, 0) * 0.8f * 0.75f);
            }
        }

        public override void PostDrawInWorld (SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            => Item.BasicInWorldGlowmask(spriteBatch, mailGlowmask.Value.Value, new Color(255, 255, 255, 0) * 0.8f * 0.75f, rotation, scale);

        public override void SetDefaults () {
            int width = 34; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 18;
        }

        public override void UpdateEquip (Player player) {
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.GetDamage(DamageClass.Ranged) += 0.05f;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedPlateMail)
               .AddRecipeGroup(RecipeGroups.Titanium, 12)
                .AddIngredient(ItemID.SoulofSight, 15)
                .AddIngredient<SoulofBlight>(15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}