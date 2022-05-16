using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Melee
{
    [AutoloadEquip(EquipType.Head)]
    public class DragonMask : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dragon Mask");
            Tooltip.SetDefault("10% increased melee damage" + "\n10% increased melee speed");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 20;
        }

        public override void UpdateEquip(Player player) {
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
            => body.type == ModContent.ItemType<DragonBreastplate>() || body.type == ModContent.ItemType<AncientDragonBreastplate>()
            && legs.type == ModContent.ItemType<DragonGreaves>() || legs.type == ModContent.ItemType<AncientDragonGreaves>();

        public override void ArmorSetShadows(Player player) 
            => player.armorEffectDrawShadowLokis = true;

        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Creates a burst of flames after taking damage";
            player.GetModPlayer<DragonPlayer>().dragonBurst = true;
        }
        
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedMask)
               .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class DragonPlayer : ModPlayer
    {
        public bool dragonBurst;

        public override void ResetEffects()
            => dragonBurst = false;

        private float projectilesCount = Main.rand.Next(3, 5);
        private float rotation = MathHelper.ToRadians(15);
        private Vector2 velocity, position;

        public override void OnHitByNPC(NPC npc, int damage, bool crit) {
            if (dragonBurst) {
                // Vector2 velocity = Helper.VelocityToPoint(Player.position, npc.position, 6f);
                velocity = new(6 * Player.direction, 0);
                position = new(Player.direction > 0 ? Player.Center.X + 15 : Player.Center.X - 15, Player.Center.Y - 5);
                for (int i = 0; i < projectilesCount; i++) {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (projectilesCount - 1))) * 1.1f;
                    Projectile.NewProjectile(Player.GetSource_OnHurt(npc, null), position, perturbedSpeed, ModContent.ProjectileType<DragonFlame>(), 50, 4.5f, Player.whoAmI);
                }
                SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot, Player.Center);
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit) {
            if (dragonBurst) {
                //Vector2 velocity = Helper.VelocityToPoint(Player.position, proj.position, 6f);
                velocity = new(6 * Player.direction, 0);
                position = new(Player.direction > 0 ? Player.Center.X + 15 : Player.Center.X - 15, Player.Center.Y - 5);
                for (int i = 0; i < projectilesCount; i++) {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (projectilesCount - 1))) * 1.1f;
                    Projectile.NewProjectile(Player.GetSource_OnHurt(proj, null), position, perturbedSpeed, ModContent.ProjectileType<DragonFlame>(), 50, 4.5f, Player.whoAmI);
                }
                SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot, Player.Center);
            }
        }
    }
}
