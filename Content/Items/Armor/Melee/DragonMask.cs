using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Melee {
    [AutoloadEquip(EquipType.Head)]
    public class DragonMask : ModItem {
        public static LocalizedText SetBonusText {
            get; private set;
        }

        public override void SetStaticDefaults () {
            ItemID.Sets.ShimmerTransformToItem [Type] = ModContent.ItemType<AncientDragonMask>();
            SetBonusText = this.GetLocalization("SetBonus");
        }

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 18;
        }

        public override void UpdateEquip (Player player) {
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
            => (body.type == ModContent.ItemType<DragonBreastplate>() || body.type == ModContent.ItemType<AncientDragonBreastplate>())
            && (legs.type == ModContent.ItemType<DragonGreaves>() || legs.type == ModContent.ItemType<AncientDragonGreaves>());

        public override void ArmorSetShadows (Player player)
            => player.armorEffectDrawShadow = true;

        public override void UpdateArmorSet (Player player) {
            player.setBonus = SetBonusText.ToString();
            player.GetModPlayer<DragonPlayer>().dragonBurst = true;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedMask)
               .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .DisableDecraft()
                .Register();
        }
    }

    public class DragonPlayer : ModPlayer {
        public bool dragonBurst;

        private bool startFlames;
        private int burstTimer;

        public override void ResetEffects ()
            => dragonBurst = false;

        public override void PostHurt (Player.HurtInfo info) {
            if (dragonBurst && !startFlames) {
                startFlames = true;
                SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot with { Volume = 0.8f, MaxInstances = 3 }, Player.Center);
            }
        }

        public override void PostUpdate () {
            if (!dragonBurst) return;

            if (startFlames) {
                burstTimer++;
                Vector2 cursorPosition = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y - 40);
                Vector2 velocity = new Vector2(cursorPosition.X - Player.position.X, cursorPosition.Y - Player.position.Y);
                int xOffset = 5;
                Vector2 position = new Vector2(Player.direction > 0 ? Player.Center.X + xOffset : Player.Center.X - xOffset, Player.Center.Y - 5);
                velocity.Normalize();
                velocity *= 5.75f;
                float rotation = MathHelper.ToRadians(15);
                float projectilesCount = 5;
                ushort type = (ushort) ModContent.ProjectileType<ShadowflameBurst>();
                if (burstTimer % 5 == 0) {
                    for (int i = 0; i < projectilesCount; i++) {
                        Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (projectilesCount))) * 1.15f;
                        Projectile.NewProjectile(Player.GetSource_Misc("Dragon Armor"), position, new Vector2(perturbedSpeed.X + Main.rand.NextFloat(-0.25f, 0.25f), perturbedSpeed.Y), type, 180, 2.5f, Player.whoAmI, 0, i);
                    }
                }
                if (velocity.X < 0.3f) Player.direction = -1;
                else Player.direction = 1;
            }

            if (burstTimer >= 30) {
                startFlames = false;
                burstTimer = 0;
            }
        }
    }
}