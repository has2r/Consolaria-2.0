using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Consolaria.Content.Items.Vanity;

namespace Consolaria.Content.Items.Consumables {
    public class RedEnvelope : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults () {
            int width = 24; int height = width;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.rare = ItemRarityID.Orange;
        }

        public override bool CanRightClick ()
            => true;

        public override void RightClick (Player player) {
            int mainDrops = Main.rand.Next(100);

            if (mainDrops == 0) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 1) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 2) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 3) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 4) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 5) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 6) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 7) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 8) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
            if (mainDrops == 59) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<Squib>(), Main.rand.Next(10, 20));
        
            if (mainDrops == 9) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 10) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 11) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 12) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 13) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 14) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 15) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 16) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 17) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            if (mainDrops == 60) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.ReleaseLantern, Main.rand.Next(3, 10));
            
            if (mainDrops == 18) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 19) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 20) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 21) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 22) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 23) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 24) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 25) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 26) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            if (mainDrops == 69) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.RedRocket, Main.rand.Next(3, 15));
            
            if (mainDrops == 27) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 28) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 29) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 30) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 31) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 32) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 33) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 34) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 35) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            if (mainDrops == 70) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.GreenRocket, Main.rand.Next(3, 15));
            
            if (mainDrops == 36) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 37) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 38) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 39) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 40) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 41) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 42) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 43) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 44) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            if (mainDrops == 79) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.BlueRocket, Main.rand.Next(3, 15));
            
            if (mainDrops == 45) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 46) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 47) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 48) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 49) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 50) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 51) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 52) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 53) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            if (mainDrops == 80) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.YellowRocket, Main.rand.Next(3, 15));
            
            if (mainDrops == 54) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FireworksBox, Main.rand.Next(1, 1));
            if (mainDrops == 55) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FireworksBox, Main.rand.Next(1, 1));
            if (mainDrops == 56) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FireworksBox, Main.rand.Next(1, 1));
            if (mainDrops == 57) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FireworksBox, Main.rand.Next(1, 1));
            if (mainDrops == 58) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FireworksBox, Main.rand.Next(1, 1));
            if (mainDrops == 99) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FireworksBox, Main.rand.Next(1, 1));
            if (mainDrops == 89) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FireworksBox, Main.rand.Next(1, 1));
            if (mainDrops == 90) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.FireworksBox, Main.rand.Next(1, 1));
            
            if (mainDrops == 61) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 61) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>(), Main.rand.Next(1, 1));
            if (mainDrops == 62) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 62) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>(), Main.rand.Next(1, 1));
            if (mainDrops == 63) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 63) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>(), Main.rand.Next(1, 1));
            if (mainDrops == 64) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 64) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>(), Main.rand.Next(1, 1));
            if (mainDrops == 65) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 65) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>(), Main.rand.Next(1, 1));
            if (mainDrops == 66) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 66) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>(), Main.rand.Next(1, 1));
            if (mainDrops == 67) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 67) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>(), Main.rand.Next(1, 1));
            if (mainDrops == 68) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalLionMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 68) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalRobe>(), Main.rand.Next(1, 1));
            
            if (mainDrops == 71) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GuaPiMao>(), Main.rand.Next(1, 1));
            if (mainDrops == 71) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 71) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 72) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GuaPiMao>(), Main.rand.Next(1, 1));
            if (mainDrops == 72) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 72) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 73) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GuaPiMao>(), Main.rand.Next(1, 1));
            if (mainDrops == 73) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 73) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 74) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GuaPiMao>(), Main.rand.Next(1, 1));
            if (mainDrops == 74) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 74) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 75) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GuaPiMao>(), Main.rand.Next(1, 1));
            if (mainDrops == 75) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 75) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 76) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GuaPiMao>(), Main.rand.Next(1, 1));
            if (mainDrops == 76) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 76) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 77) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GuaPiMao>(), Main.rand.Next(1, 1));
            if (mainDrops == 77) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 77) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 78) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<GuaPiMao>(), Main.rand.Next(1, 1));
            if (mainDrops == 78) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 78) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangSuitPants>(), Main.rand.Next(1, 1));
            
            if (mainDrops == 81) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 81) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 81) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 82) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 82) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 82) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 83) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 83) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 83) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 84) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 84) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 84) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 85) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 85) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 85) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 86) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 86) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 86) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 87) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 87) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 87) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 88) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogMask>(), Main.rand.Next(1, 1));
            if (mainDrops == 88) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 88) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<MythicalDogPants>(), Main.rand.Next(1, 1));
            
            if (mainDrops == 91) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanHat>(), Main.rand.Next(1, 1));
            if (mainDrops == 91) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 91) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 92) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanHat>(), Main.rand.Next(1, 1));
            if (mainDrops == 92) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 92) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 93) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanHat>(), Main.rand.Next(1, 1));
            if (mainDrops == 93) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 93) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 94) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanHat>(), Main.rand.Next(1, 1));
            if (mainDrops == 94) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 94) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 95) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanHat>(), Main.rand.Next(1, 1));
            if (mainDrops == 95) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 95) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 96) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanHat>(), Main.rand.Next(1, 1));
            if (mainDrops == 96) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 96) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 97) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanHat>(), Main.rand.Next(1, 1));
            if (mainDrops == 97) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 97) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanPants>(), Main.rand.Next(1, 1));
            if (mainDrops == 98) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanHat>(), Main.rand.Next(1, 1));
            if (mainDrops == 98) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanShirt>(), Main.rand.Next(1, 1));
            if (mainDrops == 98) player.QuickSpawnItem(player.GetSource_OpenItem(Type), ModContent.ItemType<TangYuanPants>(), Main.rand.Next(1, 1));

            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemID.SilverCoin, Main.rand.Next(5, 15));
        }
    }
}