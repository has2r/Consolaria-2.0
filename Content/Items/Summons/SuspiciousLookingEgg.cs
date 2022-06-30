using Consolaria.Content.NPCs.Lepus;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Summons
{
    public class SuspiciousLookingEgg : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Suspicious Looking Egg");
            Tooltip.SetDefault("Summons Lepus");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults() {
            int width = 26; int height = 28;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 20;

            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Blue;

            Item.useAnimation = 30;
            Item.useTime = 30;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
         => !NPC.AnyNPCs(ModContent.NPCType<Lepus>());
        
        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */ {
            if (player.whoAmI == Main.myPlayer) {
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = ModContent.NPCType<Lepus>();
                if (Main.netMode != NetmodeID.MultiplayerClient)               
                    NPC.SpawnOnPlayer(player.whoAmI, type);              
                else                
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);                
            }
            return true;
        }
    }
}