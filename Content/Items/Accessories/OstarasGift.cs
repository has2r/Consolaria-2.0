using Consolaria.Content.NPCs.Bosses.Lepus;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Accessories {
    [AutoloadEquip(EquipType.Waist)]
    public class OstarasGift : ModItem {
        public override void SetStaticDefaults() {

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            int width = 30; int height = width;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Blue;

            Item.expert = true;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
            => player.GetModPlayer<OstarasGiftPlayer>().chocolateEgg = true;
    }

    internal class OstarasGiftPlayer : ModPlayer {
        public bool chocolateEgg;

        public override void ResetEffects()
            => chocolateEgg = false;

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */ {
            if (chocolateEgg && target.type != ModContent.NPCType<ChocolateEgg>() && target.life <= 0 && !NPCID.Sets.CountsAsCritter[target.type] &&
                Main.rand.NextChance(0.4)) {
                int whoAmI = NPC.NewNPC(target.GetSource_Loot(), (int)target.Center.X, (int)target.Center.Y, ModContent.NPCType<ChocolateEgg>());
                if (Main.netMode == NetmodeID.Server && whoAmI < Main.maxNPCs) {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, whoAmI, 0f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */ {
            if (chocolateEgg && target.type != ModContent.NPCType<ChocolateEgg>() && target.life <= 0 && !NPCID.Sets.CountsAsCritter[target.type] &&
                Main.rand.NextChance(0.3)) {
                int whoAmI = NPC.NewNPC(target.GetSource_Loot(), (int)target.Center.X, (int)target.Center.Y, ModContent.NPCType<ChocolateEgg>());
                if (Main.netMode == NetmodeID.Server && whoAmI < Main.maxNPCs) {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, whoAmI, 0f, 0f, 0f, 0, 0, 0);
                }
            }
        }
    }
}