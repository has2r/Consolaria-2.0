using Consolaria.Content.Items.Materials;
using Consolaria.Content.Items.Weapons.Magic;
using Consolaria.Content.Items.Weapons.Melee;
using Consolaria.Content.Items.Weapons.Ranged;
using Consolaria.Content.Items.Weapons.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.BossDrops.Ocram {
    public class OcramBag : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Treasure Bag (Ocram)");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

            ItemID.Sets.BossBag [Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 3;
        }

        public override void SetDefaults () {
            int width = 24; int height = width;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.rare = ItemRarityID.Lime;
            Item.expert = true;
        }

        public override bool CanRightClick ()
            => true;

        public override void ModifyItemLoot (ItemLoot itemLoot) {
            itemLoot.Add(new OneFromRulesRule(1, ItemDropRule.Common(ModContent.ItemType<EternityStaff>()),
                ItemDropRule.Common(ModContent.ItemType<DragonBreath>()),
                ItemDropRule.Common(ModContent.ItemType<OcramsEye>()),
                ItemDropRule.Common(ModContent.ItemType<Tizona>())));

            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OcramMask>(), 8));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulofBlight>(), 1, 25, 40));

            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShadowboundExoskeleton>()));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<NPCs.Bosses.Ocram.Ocram>()));
        }

        public override Color? GetAlpha (Color lightColor)
            => Color.Lerp(lightColor, Color.White, 0.4f);

        public override void PostUpdate () {
            Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.4f);

            if (Item.timeSinceItemSpawned % 12 == 0) {
                Vector2 center = Item.Center + new Vector2(0f, Item.height * -0.1f);
                Vector2 direction = Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                Dust dust = Dust.NewDustPerfect(center + direction * distance, DustID.SilverFlame, velocity);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
            }
        }

        public override bool PreDrawInWorld (SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            Texture2D texture = TextureAssets.Item [Item.type].Value;
            Rectangle frame;

            if (Main.itemAnimations [Item.type] != null) frame = Main.itemAnimations [Item.type].GetFrame(texture, Main.itemFrameCounter [whoAmI]);
            else frame = texture.Frame();

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f) time = 2f - time;
            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.25f) {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(147, 112, 219, 50), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f) {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(138, 43, 226, 80), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}