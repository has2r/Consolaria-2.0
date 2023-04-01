using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Materials {
    public class RainbowPiece : ModItem {
        public override void SetStaticDefaults () {
            // DisplayName.SetDefault("Rainbow Piece");
            // Tooltip.SetDefault("Combine 5 pieces to craft a pot o' gold");

            Item.ResearchUnlockCount = 50;
        }

        public override void SetDefaults () {
            int width = 20; int height = width;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 5);
        }

        public override void Update (ref float gravity, ref float maxFallSpeed) {
            if (Main.dayTime)
                return;

            if (Main.netMode != NetmodeID.Server) {
                for (int index = 0; index < 10; ++index)
                    Dust.NewDust(Item.position, Item.width, Item.height, ModContent.DustType<Dusts.RomanFlame>(), Item.velocity.X, Item.velocity.Y, 100, Main.DiscoColor, Main.rand.NextFloat(0.8f, 1.3f));
            }
            SoundEngine.PlaySound(SoundID.Item4, Item.position);
            Item.TurnToAir();
        }

        public override void PostUpdate () {
            Lighting.AddLight(Item.Center, Main.DiscoColor.ToVector3() * 0.4f);

            if (Item.timeSinceItemSpawned % 12 == 0) {
                if (Main.netMode != NetmodeID.Server) {
                    Vector2 center = Item.Center + new Vector2(0f, Item.height * -0.1f);
                    Vector2 direction = Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f);
                    float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                    Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                    int dust = Dust.NewDust(center + direction * distance, 0, 0, ModContent.DustType<Dusts.RomanFlame>(), velocity.X, velocity.Y, 0, Main.DiscoColor);
                    Main.dust [dust].position = center + direction * distance;
                    Main.dust [dust].velocity = velocity;
                    Main.dust [dust].scale = 0.5f;
                    Main.dust [dust].fadeIn = 1.1f;
                    Main.dust [dust].noGravity = true;
                    Main.dust [dust].alpha = 0;
                }
            }
        }

        public override bool PreDrawInWorld (SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            Texture2D texture = TextureAssets.Item [Item.type].Value;
            Rectangle frame;

            Color rainbowGlow = new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B, 60);

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
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, rainbowGlow, rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f) {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, rainbowGlow, rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}