using Terraria.ID;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Consolaria.Content.Items.Consumables {
    public class HolyHandgrenade2 : ModItem {
        public override void SetStaticDefaults () {

            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults () {
            int width = 26; int height = 30;
            Item.Size = new Vector2(width, height);

            Item.DamageType = DamageClass.Generic;
            Item.damage = 600;

            Item.maxStack = 9999;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = Item.useTime = 50;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Lime;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Friendly.HolyHandgrenade2>();
            Item.shootSpeed = 4f;
        }

        public override bool PreDrawInWorld (SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            Texture2D texture = TextureAssets.Item [Item.type].Value;
            Rectangle frame;

            if (Main.itemAnimations [Item.type] != null) frame = Main.itemAnimations [Item.type].GetFrame(texture, Main.itemFrameCounter [whoAmI]);
            else frame = texture.Frame();

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f) time = 2f - time;
            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.25f) {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 6f).RotatedBy(radians) * time, frame, new Color(155, 155, 0, 0), 0, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f) {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 3f).RotatedBy(radians) * time, frame, new Color(155, 100, 0, 0), 0, frameOrigin, scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}