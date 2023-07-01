using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Consolaria.Content.Projectiles;

public class WormTombstone : ModProjectile {
    public override void SetDefaults() {
        Projectile.width = 30;
        Projectile.height = 32;
        Projectile.aiStyle = -1;
        Projectile.penetrate = -1;
        Projectile.knockBack = 12f;
    }

    public override void AI() {
        if (Projectile.velocity.Y == 0f) {
            Projectile.velocity.X *= 0.98f;
        }
        Projectile.rotation += Projectile.velocity.X * 0.1f;
        Projectile.velocity.Y += 0.2f;
        if (Projectile.owner != Main.myPlayer) {
            return;
        }
        int x = (int)((Projectile.position.X + Projectile.width / 2) / 16f);
        int y = (int)((Projectile.position.Y + Projectile.height - 4f) / 16f);
        if (Main.tile[x, y].HasTile) {
            return;
        }
        int tileType = ModContent.TileType<WormTombstoneTile>();
        WorldGen.PlaceTile(x, y, tileType, mute: false, forced: false, Projectile.owner);
        if (Main.tile[x, y].HasTile) {
            if (Main.netMode != NetmodeID.SinglePlayer) {
                NetMessage.SendData(MessageID.SpawnTileData, -1, -1, null, 1, x, y, tileType, 0);
            }
            int sign = Sign.ReadSign(x, y);
            if (sign >= 0) {
                Sign.TextSign(sign, Projectile.miscText);
            }
            Projectile.Kill();
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        if (Projectile.velocity.X != oldVelocity.X) {
            Projectile.velocity.X = oldVelocity.X * -0.75f;
        }
        if (Projectile.velocity.Y != oldVelocity.Y && Projectile.velocity.Y > 1.5) {
            Projectile.velocity.Y = oldVelocity.Y * -0.7f;
        }
        return false;
    }
}

public class WormTombstoneTile : ModTile {
    public override void SetStaticDefaults() {
        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        Main.tileSign[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;

        LocalizedText name = CreateMapEntryName();
        AddMapEntry(new Color(192, 192, 192), name);

        DustType = DustID.Stone;

        AdjTiles = new int[] { TileID.Tombstones };

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.CoordinateHeights = new int[2] { 16, 18 };
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.DrawYOffset = 2;

        TileObjectData.addTile(Type);
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        => true;

    public override bool RightClick(int i, int j)
        => true;

    public override void NumDust(int i, int j, bool fail, ref int num)
        => num = fail ? 1 : 3;

    public override void KillMultiTile(int i, int j, int frameX, int frameY) {
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<WormTombstoneItem>());
        Sign.KillSign(i, j);
    }

    public override void MouseOverFar(int i, int j)
        => MouseOver(i, j);
}

public class WormTombstoneItem : ModItem {
    public override string Texture
        => $"Consolaria/Content/Projectiles/WormTombstone";

    public override void SetDefaults() {
        Item.CloneDefaults(ItemID.Tombstone);
        Item.rare = ItemRarityID.Yellow;

        Item.DefaultToPlaceableTile(ModContent.TileType<WormTombstoneTile>());
    }
}