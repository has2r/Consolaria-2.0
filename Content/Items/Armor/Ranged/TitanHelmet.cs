using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged {

    [AutoloadEquip(EquipType.Head)]
    public class TitanHelmet : ModItem {

        public static Lazy<Asset<Texture2D>> helmetGlowmask;
        public override void Unload () => helmetGlowmask = null;

        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Titan Helmet");
            Tooltip.SetDefault("15% increased ranged damage and critical strike chance" + "\n25% chance to not consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 1;

            if (!Main.dedServ) {
                helmetGlowmask = new(() => ModContent.Request<Texture2D>(Texture + "_Glow"));
                HeadGlowmask.RegisterData(Item.headSlot, new DrawLayerData() {
                    Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
                });
            }
        }

        public override void PostDrawInWorld (SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            => Item.BasicInWorldGlowmask(spriteBatch, helmetGlowmask.Value.Value, new Color(255, 255, 255, 0) * 0.8f, rotation, scale);

        public override void SetDefaults () {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 14;
        }

        public override void UpdateEquip (Player player) {
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
        }

        public override bool IsArmorSet (Item head, Item body, Item legs)
            => (body.type == ModContent.ItemType<TitanMail>() || body.type == ModContent.ItemType<AncientTitanMail>())
            && (legs.type == ModContent.ItemType<TitanLeggings>() || legs.type == ModContent.ItemType<AncientTitanLeggings>());

        public override void ArmorSetShadows (Player player)
            => player.armorEffectDrawOutlinesForbidden = true;

        public override void UpdateArmorSet (Player player) {
            player.setBonus = "Using ranged weapons emits strong repelling wave around you";
            player.GetModPlayer<TitanPlayer>().titanPower = true;
        }

        public override void AddRecipes () {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedHelmet)
                .AddRecipeGroup(RecipeGroups.Titanium, 10)
                .AddIngredient(ItemID.SoulofSight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    internal class TitanPlayer : ModPlayer {
        public bool titanPower;
        public int shockwaveTimer;
        public readonly int shockwaveTimerLimit = 300;

        public override void Initialize () 
           => shockwaveTimer = shockwaveTimerLimit;

        public override void ResetEffects () 
           => titanPower = false;
           
        public override void PostUpdateEquips () {
            if (!titanPower) return;
            if (shockwaveTimer > 0)
                shockwaveTimer--;
        }
    }
          
    internal class TitanArmorBonuses : GlobalItem {
        public override bool? UseItem (Item item, Player player) {
            TitanPlayer modPlayer = player.GetModPlayer<TitanPlayer>();
            ushort type = (ushort) ModContent.ProjectileType<TitanBlast>();

            if (modPlayer.titanPower && player.ownedProjectileCounts [type] < 1 && item.DamageType == DamageClass.Ranged &&
                modPlayer.shockwaveTimer == 0) {
                Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, new Vector2(0, 0), type, 80, 7.5f, player.whoAmI);
                //SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/Shockwave") { Volume = 0.8f }, player.position);
                modPlayer.shockwaveTimer = modPlayer.shockwaveTimerLimit;
            }
            return null;
        }

		public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            TitanPlayer modPlayer = player.GetModPlayer<TitanPlayer>();
            ushort type2 = (ushort)ModContent.ProjectileType<TitanBlast>();

            if (modPlayer.titanPower && player.ownedProjectileCounts[type2] < 1 && item.DamageType == DamageClass.Ranged &&
                modPlayer.shockwaveTimer == 0)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity, type2, damage * 10, 7.5f, player.whoAmI);
                //SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/Shockwave") { Volume = 0.8f }, player.position);
                modPlayer.shockwaveTimer = modPlayer.shockwaveTimerLimit;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}

		public override bool CanConsumeAmmo (Item weapon, Item ammo, Player player) {
            float dontConsumeAmmoChance = 0f;
            if (weapon.useAmmo >= 0) {
                if (player.armor [0].type == ModContent.ItemType<TitanHelmet>() || player.armor [0].type == ModContent.ItemType<AncientTitanHelmet>())
                    dontConsumeAmmoChance += 0.25f;
                if (player.armor [1].type == ModContent.ItemType<TitanMail>() || player.armor [1].type == ModContent.ItemType<AncientTitanMail>()) 
                    dontConsumeAmmoChance += 0.2f;
                if (player.armor [2].type == ModContent.ItemType<TitanLeggings>() || player.armor [2].type == ModContent.ItemType<AncientTitanLeggings>()) 
                    dontConsumeAmmoChance += 0.15f;
                return Main.rand.NextFloat() >= dontConsumeAmmoChance;
            }
            return true;
        }
    }
}