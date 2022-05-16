using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Consolaria.Content.Items.BossDrops.Lepus
{
    public class LepusBag : ModItem
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.Lepus.Lepus>();

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Treasure Bag (Lepus)");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

            ItemID.Sets.BossBag[Type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults() {
            int width = 24; int height = width;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 999;
            Item.consumable = true;

            Item.rare = ItemRarityID.Blue;
            Item.expert = true;
        }

        public override bool CanRightClick() 
         => true;
        
        public override void OpenBossBag(Player player) {
           /* int armorDrop = Main.rand.Next(3);
            if (armorDrop == 0)
            {
                player.QuickSpawnItem(mod.ItemType("OstaraHat"));
            }
            if (armorDrop == 1)
            {
                player.QuickSpawnItem(mod.ItemType("OstaraChainmail"));
            }
            if (armorDrop == 2)
            {
                player.QuickSpawnItem(mod.ItemType("OstaraBoots"));
            }*/

            if (Main.rand.Next(2) == 0)
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<EggCannon>());         
            if (Main.rand.Next(8) == 0)
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<LepusMask>());           
            if (Main.rand.Next(10) == 0)
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BunnyHood);
            
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<SuspiciousLookingEgg>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<OstarasGift>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GoldCoin, 3);
        }

        public override Color? GetAlpha(Color lightColor)
            => Color.Lerp(lightColor, Color.White, 0.4f);

        public override void PostUpdate() {
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

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Rectangle frame;

            if (Main.itemAnimations[Item.type] != null) frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
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
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(255, 105, 180, 50), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f) {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(140, 120, 255, 77), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
