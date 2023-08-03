using Consolaria.Content.Buffs;
using Consolaria.Content.NPCs.Bosses.Ocram;
using Consolaria.Content.NPCs.Bosses.Turkor;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Summons {
    public class CursedStuffing : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns [Type] = 12;
        }

        public override void SetDefaults () {
            int width = 34; int height = 28;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 9999;

            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;

            Item.useAnimation = 30;
            Item.useTime = 30;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem (Player player)
            => player.HasBuff(ModContent.BuffType<PetTurkey>()) && !NPC.AnyNPCs(ModContent.NPCType<TurkortheUngrateful>());

        public override bool? UseItem (Player player) {
            player.ClearBuff(ModContent.BuffType<PetTurkey>());
            int type = ModContent.NPCType<TurkortheUngrateful>();
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                int npc = NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)player.Center.Y, type);
                Main.npc[npc].target = player.whoAmI;
                if (Main.netMode == NetmodeID.Server && npc < Main.maxNPCs) {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc);
                }
            }
            return true;
        }
    }
}