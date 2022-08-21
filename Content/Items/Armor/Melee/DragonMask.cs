using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Melee {
    [AutoloadEquip(EquipType.Head)]
    public class DragonMask : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Dragon Mask");
            Tooltip.SetDefault("10% increased melee damage" + "\n10% increased melee speed");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
        }

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 20;
        }

        public override void UpdateEquip (Player player) {
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
            => body.type == ModContent.ItemType<DragonBreastplate>() || body.type == ModContent.ItemType<AncientDragonBreastplate>()
            && legs.type == ModContent.ItemType<DragonGreaves>() || legs.type == ModContent.ItemType<AncientDragonGreaves>();

        public override void ArmorSetShadows (Player player)
            => player.armorEffectDrawShadow = true;

        public override void UpdateArmorSet (Player player) {
            player.setBonus = "Creates a burst of flames after taking damage";
            player.GetModPlayer<DragonPlayer>().dragonBurst = true;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedMask)
               .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class DragonPlayer : ModPlayer {
        public bool dragonBurst;

        public override void ResetEffects ()
            => dragonBurst = false;

        public override void OnHitByNPC (NPC npc, int damage, bool crit) {
            if (dragonBurst)
                ShootFlames(Player.GetSource_OnHurt(npc, null));
        }

        public override void OnHitByProjectile (Projectile proj, int damage, bool crit) {
            if (dragonBurst)
                ShootFlames(Player.GetSource_OnHurt(proj, null));
        }

        private void ShootFlames (IEntitySource spawnSource) {
            float projectilesCount = Main.rand.Next(3, 5);
            float rotation = MathHelper.ToRadians(15);
            Vector2 velocity, position;
            ushort type = (ushort)ModContent.ProjectileType<DragonFlame>();
            velocity = new Vector2(6 * Player.direction, 0);
            position = new Vector2(Player.direction > 0 ? Player.Center.X + 15 : Player.Center.X - 15, Player.Center.Y - 5);
            for (int i = 0; i < projectilesCount; i++) {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (projectilesCount - 1))) * 1.1f;
                Projectile.NewProjectile(spawnSource, position, new Vector2(perturbedSpeed.X + Main.rand.Next(-2, 2), perturbedSpeed.Y), type, 50, 4.5f, Player.whoAmI);
            }
            SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot, Player.Center);
        }
    }
}