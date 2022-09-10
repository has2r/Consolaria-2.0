using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Magic {
    [AutoloadEquip(EquipType.Head)]
    public class PhantasmalHeadgear : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Phantasmal Headgear");
            Tooltip.SetDefault("10% increased magic damage and critical strike chance" + "\nIncreases maximum mana by 50");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;
        }

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 12;
        }

        public override void UpdateEquip (Player player) {
            player.statManaMax2 += 50;

            player.GetCritChance(DamageClass.Magic) += 10;
            player.GetDamage(DamageClass.Magic) += 0.1f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
           => (body.type == ModContent.ItemType<PhantasmalRobe>() || body.type == ModContent.ItemType<AncientPhantasmalRobe>())
           && (legs.type == ModContent.ItemType<PhantasmalSubligar>() || legs.type == ModContent.ItemType<AncientPhantasmalSubligar>());

        public override void ArmorSetShadows (Player player)
            => player.armorEffectDrawOutlines = true;

        public override void UpdateArmorSet (Player player) {
            player.setBonus = "Siphons mana out of nearby enemies";
            player.GetModPlayer<SpectralPlayer>().spectralGuard = true;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedHeadgear)
                .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofFright, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    internal class SpectralPlayer : ModPlayer {
        public bool spectralGuard;
        public int absorptionRadius = 200;

        private bool spectralAura;
        private int activeTimer;

        public override void ResetEffects ()
            => spectralGuard = false;

        public override void PreUpdate () {
            if (!spectralGuard) return;

            if (Player.ownedProjectileCounts [ModContent.ProjectileType<SpectralSpirit>()] >= 3)
                spectralAura = true;
            else spectralAura = false;

            if (Player.statMana < Player.statManaMax2 * 0.75f) {
                if (!spectralAura && Player.whoAmI == Main.myPlayer) {
                    int randomPosition = Main.rand.Next(-20, 21);
                    for (int i = 0; i < 3; i++)
                        Projectile.NewProjectile(Player.GetSource_Misc("Spectral Armor"), Player.MountedCenter.X + randomPosition, Player.MountedCenter.Y + randomPosition, 0f, 0f, ModContent.ProjectileType<SpectralSpirit>(), 0, 0f, Player.whoAmI, 1 * i, 0);
                    SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, Player.Center);
                }
            }

            if (activeTimer > 0) activeTimer--;
            if (spectralAura && activeTimer <= 0 && Player.statMana < Player.statManaMax2 && Player.whoAmI == Main.myPlayer) {
                for (int _findNPC = 0; _findNPC < Main.npc.Length; _findNPC++) {
                    NPC npc = Main.npc [_findNPC];
                    if (npc.active && !npc.friendly && npc.life > 0 && Vector2.Distance(Player.Center, npc.Center) < absorptionRadius) {
                        Projectile.NewProjectile(Player.GetSource_Misc("Spectral Armor"), npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<ManaDrain>(), 0, 0f, Player.whoAmI);
                        activeTimer = 20;
                    }
                }
            }
        }
    }
}