using Consolaria.Content.NPCs.Friendly.McMoneypants;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Friendly.McMoneypants;

public class McMoneypantsInvitation : ModItem {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;

        ItemID.Sets.SortingPriorityBossSpawns[Type] = 1;
    }

    public override void SetDefaults() {
        int width = 30; int height = 20;
        Item.Size = new Vector2(width, height);

        Item.maxStack = 1;
        Item.UseSound = SoundID.NPCHit2;

        Item.value = Item.sellPrice(gold: 3);
        Item.rare = ItemRarityID.Orange;

        Item.useAnimation = 18;
        Item.useTime = 18;

        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.consumable = true;
    }

    public override bool? UseItem(Player player) {
        string text = "The invitation flies away to its recipient...";
        if (player.whoAmI == Main.myPlayer && player.itemAnimation >= player.itemAnimationMax) {
            if (Main.netMode == NetmodeID.SinglePlayer) {
                Main.NewText(text, Color.HotPink);
            }
            else if (Main.netMode == NetmodeID.Server) {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text), Color.HotPink);
            }
            NPC.SetEventFlagCleared(ref McMoneypantsWorldData.GildedInvitationUsed, -1);
            if (Main.netMode == NetmodeID.Server) {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        return true;
    }
}