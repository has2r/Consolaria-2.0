using Consolaria.Content.NPCs.Bosses.Turkor;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets {
    public class TurkeyFeather : PetItem {
        public override void SetStaticDefaults() {

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.Friendly.Pets.PetTurkey>(), ModContent.BuffType<Buffs.PetTurkey>());

            int width = 46; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 10);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            int buffType = Item.buffType;
            if (!NPC.AnyNPCs(ModContent.NPCType<TurkortheUngrateful>())) {
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    player.AddBuff(buffType, 2);
                    Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
                }
                else {
                    NetMessage.SendData(MessageID.AddPlayerBuff, number: player.whoAmI, number2: buffType);
                    NetMessage.SendData(MessageID.SyncProjectile, number: player.whoAmI, number2: type);
                }
            }
            return false;
        }
    }
}