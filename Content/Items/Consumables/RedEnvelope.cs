using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using Consolaria.Content.Items.Vanity;
using Consolaria.Content.NPCs.Friendly.McMoneypants;
using Terraria.ModLoader.IO;
using Consolaria.Content.Items.Miscellaneous.Kites.Custom;

namespace Consolaria.Content.Items.Consumables;

public class RedEnvelope : ModItem {
    public override void SetStaticDefaults()
        => Item.ResearchUnlockCount = 3;

    public override void SetDefaults() {
        int width = 24; int height = width;
        Item.Size = new Vector2(width, height);

        Item.maxStack = 9999;
        Item.consumable = true;

        Item.rare = ItemRarityID.Orange;
    }

    public override bool CanRightClick()
        => true;

    public override void RightClick(Player player) {
        if (Main.rand.NextChance(0.04)) {
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<McMoneypantsInvitation>());
        }

        player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.SilverCoin, Main.rand.Next(5, 15));

        float chance = (float)Math.Round(Main.rand.NextDouble(), 1);

        if (chance > 0.6f) {
            if (!Main.rand.NextChance(0.3)) {
                chance = (float)Math.Round((double)Main.rand.NextFloat(0f, 0.6f), 1);
            }
        }

        Dictionary<float, (int, int)> items = new() {
            { 0f, (ModContent.ItemType<Squib>(), Main.rand.Next(10, 20)) },
            { 0.1f, (ItemID.ReleaseLantern, Main.rand.Next(3, 10)) },
            { 0.2f, (ItemID.RedRocket, Main.rand.Next(3, 15)) },
            { 0.3f, (ItemID.GreenRocket, Main.rand.Next(3, 15)) },
            { 0.4f, (ItemID.BlueRocket, Main.rand.Next(3, 15)) },
            { 0.5f, (ItemID.YellowRocket, Main.rand.Next(3, 15)) },
            { 0.6f, (ModContent.ItemType<CandiedFruit>(), 1) },
        };
        if (chance <= 0.6f) {
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), items[chance].Item1, items[chance].Item2);
            return;
        }

        if (Main.rand.NextBool()) {
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>());
            return;
        }

        player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalWyvernKite>());
    }
}