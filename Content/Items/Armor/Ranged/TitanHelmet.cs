using Consolaria.Content.Items.Materials;
using Consolaria.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Armor.Ranged
{
    [AutoloadEquip(EquipType.Head)]
    public class TitanHelmet : ModItem
    {
        public static Asset<Texture2D> helmetGlowmask;
        public override void Unload() => helmetGlowmask = null;
        

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Titan Helmet");
            Tooltip.SetDefault("15% increased ranged damage and critical strike chance" + "\n25% chance to not consume ammo");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            if (!Main.dedServ) {
                helmetGlowmask = ModContent.Request<Texture2D>(Texture + "_Glow");
                HeadGlowmask.RegisterData(Item.headSlot, new DrawLayerData() {
                    Texture = ModContent.Request<Texture2D>(Texture + "_Head_Glow")
                });
            }
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
            => Item.BasicInWorldGlowmask(spriteBatch, helmetGlowmask.Value, new Color(255, 255, 255, 0) * 0.8f * 0.75f, rotation, scale);
        
        public override void SetDefaults() {
            int width = 30; int height = 26;
            Item.Size = new Vector2(width, height);

            Item.value = Item.sellPrice(gold: 6, silver: 40);
            Item.rare = ItemRarityID.Lime;

            Item.defense = 14;
        }

        public override void UpdateEquip(Player player) {
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) 
            => body.type == ModContent.ItemType<TitanMail>() && legs.type == ModContent.ItemType<TitanLeggings>();

        public override void ArmorSetShadows(Player player)
            => player.armorEffectDrawOutlines = true;
        
        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Using ranged weapons emits strong repelling wave around you";
            player.GetModPlayer<TitanPlayer>().titanPower = true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedHelmet)
                .AddIngredient(ItemID.HellstoneBar, 12)
                .AddIngredient(ItemID.SoulofSight, 10)
                .AddIngredient<SoulofBlight>(10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class TitanPlayer : ModPlayer
    {
        public bool titanPower;

        public override void ResetEffects() 
            => titanPower = false;   
    }

    internal class TitanArmorBonuses : GlobalItem
    {
        public override bool? UseItem(Item item, Player player) {
            ushort projType = (ushort)ModContent.ProjectileType<TitanShockwawe>();
            if (player.GetModPlayer<TitanPlayer>().titanPower && player.ownedProjectileCounts[projType] < 1 && item.DamageType == DamageClass.Ranged && player.miscCounter % 10 == 0) {
                Projectile.NewProjectile(player.GetItemSource_Misc(-1), player.Center, new Vector2(0, 0), projType, 3, 9f, player.whoAmI);
                SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, player.Center);
            }
            return null;
        }

        public override bool CanConsumeAmmo(Item weapon, Player player) {
            float dontConsumeAmmoChance = 0f;
            if (weapon.useAmmo >= 0) {
                if (player.armor[0].type == ModContent.ItemType<TitanHelmet>()) dontConsumeAmmoChance += 0.25f;
                if (player.armor[1].type == ModContent.ItemType<TitanMail>()) dontConsumeAmmoChance += 0.2f;
                if (player.armor[2].type == ModContent.ItemType<TitanLeggings>()) dontConsumeAmmoChance += 0.15f;
                return Main.rand.NextFloat() >= dontConsumeAmmoChance;
            }
            return true;
        }
    }
}
