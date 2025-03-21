using Consolaria.Content.Projectiles.Friendly;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Weapons.Melee {
    public class Tonbogiri : ModItem {
        public override void SetStaticDefaults () {
            ItemID.Sets.SkipsInitialUseSound [Item.type] = true;
            ItemID.Sets.Spears [Item.type] = true;

            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults () {
            int width = 56; int height = width;
            Item.Size = new Vector2(width, height);

            Item.damage = 92;
            Item.DamageType = DamageClass.Melee;

            Item.noUseGraphic = true;
            Item.useTime = Item.useAnimation = 32;

            Item.shoot = ModContent.ProjectileType<TonbogiriSpear>();
            Item.shootSpeed = 57f;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2f;
            Item.crit = 10;

            Item.value = Item.buyPrice(gold: 5, silver: 30);
            Item.rare = ItemRarityID.Lime;

            Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.channel = true;
        }

        public override bool CanUseItem (Player player)
            => player.ownedProjectileCounts [Item.shoot] < 1;

        public override bool? UseItem (Player player) {
            if (!Main.dedServ && Item.UseSound.HasValue)
                SoundEngine.PlaySound(Item.UseSound.Value, player.position);
            return null;
        }
    }
}