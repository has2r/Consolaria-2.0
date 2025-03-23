using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Lepus {
    internal class Stomp : ModProjectile {
        public override string Texture
            => "Consolaria/Assets/Textures/Empty";

        public override string Name => "Lepus Stomp";

        public override void SetDefaults() {
            Projectile.width = 30;
            Projectile.height = 30;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;

            Projectile.timeLeft = 120;
            Projectile.alpha = 255;
        }

        public override void AI() {
            float num = Projectile.ai[1];
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 3f) {
                Projectile.Kill();
                return;
            }
            Projectile.velocity = Vector2.Zero;
            Projectile.position = Projectile.Center;
            Projectile.Size = new Vector2(16f, 8f) * MathHelper.Lerp(5f, num, Utils.GetLerpValue(0f, 9f, Projectile.ai[0]));
            Projectile.Center = Projectile.position;
            if (Main.netMode == NetmodeID.Server) {
                return;
            }
            var point = Projectile.TopLeft.ToTileCoordinates();
            Point point2 = Projectile.BottomRight.ToTileCoordinates();
            int num3 = Projectile.width / 2;
            if ((int)Projectile.ai[0] % 3 != 0)
                return;
            int num4 = (int)Projectile.ai[0] / 3;
            for (int i = point.X; i <= point2.X; i++) {
                for (int j = point.Y; j <= point2.Y; j++) {
                    if (Vector2.Distance(Projectile.Center, new Vector2(i * 16, j * 16)) > num3)
                        continue;
                    Tile tileSafely = Framing.GetTileSafely(i, j);
                    if (!tileSafely.HasTile || !Main.tileSolid[tileSafely.TileType] || Main.tileSolidTop[tileSafely.TileType] || Main.tileFrameImportant[tileSafely.TileType])
                        continue;
                    Tile tileSafely2 = Framing.GetTileSafely(i, j - 1);
                    if (tileSafely2.HasTile && Main.tileSolid[tileSafely2.TileType] && !Main.tileSolidTop[tileSafely2.TileType])
                        continue;
                    int num5 = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely, i, j);
                    for (int k = 0; k < num5; k++) {
                        Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
                        obj.velocity.Y -= 3f + num4 * 1.5f;
                        obj.velocity.Y *= Main.rand.NextFloat();
                        obj.velocity.Y *= 0.75f;
                        obj.scale += num4 * 0.03f;
                    }
                    if (num4 >= 2) {
                        for (int m = 0; m < num5 - 1; m++) {
                            Dust obj2 = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
                            obj2.velocity.Y -= 1f + num4;
                            obj2.velocity.Y *= Main.rand.NextFloat();
                            obj2.velocity.Y *= 0.75f;
                        }
                    }
                    if (num4 >= 2) {
                        for (int m = 0; m < num5 - 1; m++) {
                            Dust obj2 = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
                            obj2.velocity.Y -= 1f + num4;
                            obj2.velocity.Y *= Main.rand.NextFloat();
                            obj2.velocity.Y *= 0.75f;
                        }
                    }
                    if (num5 <= 0 || Main.rand.NextBool(3))
                        continue;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) {
            short debuffTime = 120;
            short debuffTime2 = 180;
            target.AddBuff(BuffID.Slow, Main.expertMode ? debuffTime2 : debuffTime);
        }
    }
}