using Consolaria.Content.NPCs.Friendly.McMoneypants;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Consumables;

public class McMoneypantsInvitation : ModItem {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;

        ItemID.Sets.SortingPriorityBossSpawns[Type] = 1;
    }

    public override void SetDefaults() {
        int width = 30; int height = 20;
        Item.Size = new Vector2(width, height);

        Item.maxStack = 9999;
        Item.UseSound = SoundID.Item92;

        Item.value = Item.sellPrice(gold: 3);
        Item.rare = ItemRarityID.Orange;

        Item.useAnimation = 45;
        Item.useTime = 45;

        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.consumable = true;
    }

    public override bool? UseItem(Player player) {
        if (McMoneypantsWorldData.isGildedInvitationUsed) {
            return false;
        }

        string text = Language.GetTextValue($"Mods.Consolaria.McMoneypantsInvitationUsage"); ;
        if (player.whoAmI == Main.myPlayer && player.itemAnimation >= player.itemAnimationMax) {
            if (Main.netMode == NetmodeID.SinglePlayer) {
                Main.NewText(text, Color.SpringGreen);
            }
            else if (Main.netMode == NetmodeID.Server) {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text), Color.SpringGreen);
            }
            McMoneypantsWorldData.isGildedInvitationUsed = true;
            if (Main.netMode == NetmodeID.Server) {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        return true;
    }
}