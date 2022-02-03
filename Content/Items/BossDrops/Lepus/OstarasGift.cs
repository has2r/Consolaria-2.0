using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Consolaria.Content.NPCs.Lepus;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.Items.BossDrops.Lepus
{
    public class OstarasGift : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ostara's Gift");
            Tooltip.SetDefault("Enemies have a chance of leaving chocolate eggs on death" + "\nBroken eggs drop some life hearts and mana stars");
        }

        public override void SetDefaults() {
            int width = 30; int height = width;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(0, 1, 50, 0);

            Item.rare = ItemRarityID.Blue;
            Item.expert = true;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)    
         => player.GetModPlayer<OstarasGiftPlayer>().chocolateEgg = true;
    }

    public class OstarasGiftPlayer : ModPlayer
    {
        public bool chocolateEgg;

        public override void ResetEffects()
         => chocolateEgg = false;
        
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) {
            if (chocolateEgg && target.type != ModContent.NPCType<ChocolateEgg>() && target.life <= 0 && !NPCID.Sets.CountsAsCritter[target.type] && Main.rand.Next(2) == 0)
                NPC.NewNPC((int)target.Center.X, (int)target.Center.Y, ModContent.NPCType<ChocolateEgg>());  
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) {
            if (chocolateEgg && target.type != ModContent.NPCType<ChocolateEgg>() && target.life <= 0 && !NPCID.Sets.CountsAsCritter[target.type] && Main.rand.Next(2) == 0)
                NPC.NewNPC((int)target.Center.X, (int)target.Center.Y, ModContent.NPCType<ChocolateEgg>());
        }
    }
}