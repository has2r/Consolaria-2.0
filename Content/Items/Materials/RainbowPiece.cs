using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Items.Materials
{
    public class RainbowPiece : ModItem {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Rainbow Piece");
            Tooltip.SetDefault("Combine 5 pieces to craft a pot o' gold");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId [Type] = 50;
        }

        public override void SetDefaults () {
            int width = 20; int height = width;
            Item.Size = new Vector2(width, height);

            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 5);
        }

        public override void Update (ref float gravity, ref float maxFallSpeed) {
            if (Main.dayTime)
                return;
            SoundEngine.PlaySound(SoundID.Item4, Item.position);
            for (int index = 0; index < 10; ++index)
                Dust.NewDust(Item.position, Item.width, Item.height, ModContent.DustType<Dusts.RomanFlame>(), Item.velocity.X, Item.velocity.Y, 100, Main.DiscoColor, Main.rand.NextFloat(0.8f, 1.3f));

            Item.active = false;
            Item.type = ItemID.None;
            Item.stack = 0;
        }

        public override void PostUpdate () {
            if (!Main.rand.NextBool(5))
                return;
            int dust = Dust.NewDust(Item.position, Item.width, Item.height / 2, ModContent.DustType<Dusts.RomanFlame>(), 0f, -5f, 100, Main.DiscoColor, 1.2f);
            Main.dust [dust].noGravity = true;
            Main.dust [dust].velocity.Y *= Main.rand.NextFloat(0.8f, 1.2f);
        }
    }
}
