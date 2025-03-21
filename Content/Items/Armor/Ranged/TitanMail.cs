using Consolaria.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged {
    [AutoloadEquip(EquipType.Body)]
    public class TitanMail : ModItem {
        public static Lazy<Asset<Texture2D>> mailGlowmask;
        public override void Unload () => mailGlowmask = null;

        public override void SetStaticDefaults () {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<AncientTitanMail>();

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
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
        }
    }
}