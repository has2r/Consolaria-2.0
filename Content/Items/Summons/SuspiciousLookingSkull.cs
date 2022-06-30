using Consolaria.Content.NPCs.Ocram;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Summons
{
    public class SuspiciousLookingSkull : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Suspicious Looking Skull");
            Tooltip.SetDefault("Summons Ocram");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults() {
            int width = 18; int height = 22;
            Item.Size = new Vector2(width, height);

            Item.maxStack = 20;

            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Pink;

            Item.useAnimation = 30;
            Item.useTime = 30;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
         =>  !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Ocram>());
        
        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */ {
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

        public override void AddRecipes() {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.Bone, 15)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}