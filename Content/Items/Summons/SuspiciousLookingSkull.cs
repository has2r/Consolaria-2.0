using Consolaria.Content.NPCs.Bosses.Ocram;
using Microsoft.Xna.Framework;
using System;

using Terraria;
<<<<<<< Updated upstream
using Terraria.Audio;
=======
using Terraria.GameContent.UI;
>>>>>>> Stashed changes
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Summons {
    public class SuspiciousLookingSkull : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns [Type] = 12;
        }

        public override void SetDefaults () {
            int width = 18; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 9999;

            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Lime;

            Item.useAnimation = 30;
            Item.useTime = 30;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem (Player player)
            => !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Ocram>());

        public override bool? UseItem (Player player) {
<<<<<<< Updated upstream
            if (player.whoAmI == Main.myPlayer) {
                int type = ModContent.NPCType<Ocram>();
                NPC.NewNPC(player.GetSource_FromThis(), (int)player.Center.X, (int)player.Center.Y, type);
=======
            int type = ModContent.NPCType<Ocram>();
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                int npc = NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)player.Center.Y, type);
                Main.npc[npc].target = player.whoAmI; 
                if (Main.netMode == NetmodeID.Server && npc < Main.maxNPCs) {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc);
                }
>>>>>>> Stashed changes
            }
            return true;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 15)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}