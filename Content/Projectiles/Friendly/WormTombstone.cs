using Consolaria.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly;

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