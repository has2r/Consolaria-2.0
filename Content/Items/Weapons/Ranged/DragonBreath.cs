using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Ranged {
    public class DragonBreath : ModItem {
		public override void SetStaticDefaults () 
			=> Item.ResearchUnlockCount = 1;

		public override void SetDefaults () {
			int width = 28; int height = 30;
			Item.Size = new Vector2(width, height);

			Item.damage = 36;
			Item.DamageType = DamageClass.Ranged;

            Item.useAnimation = 20;
            Item.useTime = 6;

            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.ShadowflameBreath>();
			Item.shootSpeed = 6.5f;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.5f;
			Item.useAmmo = AmmoID.Gel;

			Item.value = Item.sellPrice(gold: 5, silver: 60);
			Item.rare = ItemRarityID.Lime;

			Item.UseSound = SoundID.Item34;

			Item.autoReuse = true;
			Item.noMelee = true;
		}

        public override void ModifyShootStats (Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			Vector2 _velocity = Utils.SafeNormalize(new Vector2(velocity.X, velocity.Y), Vector2.Zero);
			position += _velocity * 30f;
			position += new Vector2(-_velocity.Y, _velocity.X) * (-2f * player.direction);
		}

		public override bool Shoot (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			int _randomCount = Main.rand.Next(1, 3);
			for (int i = 0; i < _randomCount; i++) {
				float _randomVel = Main.rand.Next(-15, 15) * 0.035f;
				velocity += new Vector2(_randomVel, _randomVel);
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
			}
			return false;
		}

		public override Vector2? HoldoutOffset ()
			=> new Vector2(-6, 0);

		public override bool CanConsumeAmmo (Item ammo, Player player)
			=> player.itemAnimation <= player.itemTimeMax && Main.rand.NextChance(0.3);

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine tooltip = new(Mod, "Flamethrower Tooltip", Language.GetTextValue("ItemTooltip.Flamethrower") + "\n" + Language.GetTextValue("ItemTooltip.CandyCornRifle").Replace("33", "70"));
            tooltips.Add(tooltip);
        }
    }
}