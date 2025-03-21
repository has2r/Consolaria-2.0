using Consolaria.Common;
using Consolaria.Content.Items.Kites.Custom;
using Consolaria.Content.Items.Vanity;
using Consolaria.Content.Items.Weapons.Throwing;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

        Dictionary<float, (int, int)> items = new()
        {
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
            if (ModLoader.TryGetMod("XDContentMod", out Mod XDContentMod) & ModContent.GetInstance<ConsolariaConfig>().heartbeatariaIntegrationEnabled) {
                Dictionary<float, int[]> vanityItems = new()
                {
                    { 0.7f, new int[] { ModContent.ItemType<MythicalLionMask>(),
                                        ModContent.ItemType<MythicalRobe>() } },
                    { 0.8f, new int[] { XDContentMod.Find<ModItem>("GuaPiMao").Type,
                                        XDContentMod.Find<ModItem>("TangSuitShirt").Type,
                                        XDContentMod.Find<ModItem>("TangSuitPants").Type } },
                    { 0.9f, new int[] { XDContentMod.Find<ModItem>("MythicalDogMask").Type,
                                        XDContentMod.Find<ModItem>("MythicalDogShirt").Type,
                                        XDContentMod.Find<ModItem>("MythicalDogPants").Type } },
                    { 1f, new int[] { XDContentMod.Find<ModItem>("TangYuanHat").Type,
                                        XDContentMod.Find<ModItem>("TangYuanShirt").Type,
                                        XDContentMod.Find<ModItem>("TangYuanPants").Type } },
                    };

                foreach (int itemIds in vanityItems[chance]) {
                    player.QuickSpawnItem(player.GetSource_OpenItem(Type), itemIds);
                }
            }

            else {
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>());
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>());
                return;
            }

            if (!ModContent.GetInstance<ConsolariaConfig>().mythicalWyvernKiteVanillaDropruleEnabled) {
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalWyvernKite>());
            }
        }
    }
}