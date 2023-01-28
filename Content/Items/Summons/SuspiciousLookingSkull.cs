using Consolaria.Content.NPCs.Bosses.Ocram;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Summons {
    public class SuspiciousLookingSkull : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Suspicious Looking Skull");
            Tooltip.SetDefault("Summons Ocram");

            SacrificeTotal = 3;
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
            if (player.whoAmI == Main.myPlayer) {
                SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/OcramRoar"), player.position);

                int type = ModContent.NPCType<Ocram>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                else
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
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