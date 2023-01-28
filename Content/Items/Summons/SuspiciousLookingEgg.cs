using Consolaria.Content.NPCs.Bosses.Lepus;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Summons {
    public class SuspiciousLookingEgg : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Suspicious Looking Egg");
            Tooltip.SetDefault("Summons Lepus");

            SacrificeTotal = 3;
            ItemID.Sets.SortingPriorityBossSpawns [Type] = 12;
        }

        public override void SetDefaults () {
            int width = 26; int height = 28;
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
            => !NPC.AnyNPCs(ModContent.NPCType<Lepus>());

        public override bool? UseItem (Player player) {
            SoundEngine.PlaySound(SoundID.Roar);

            int type = ModContent.NPCType<Lepus>();
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.SpawnOnPlayer(player.whoAmI, type);
            else
                NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
            return true;
        }
    }
}