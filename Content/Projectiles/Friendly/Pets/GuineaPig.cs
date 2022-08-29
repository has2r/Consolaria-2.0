using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class GuineaPig : ModProjectile {
        public override void SetStaticDefaults () {
            Main.projFrames [Projectile.type] = 8;
            Main.projPet [Projectile.type] = true;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.Bunny);
            AIType = ProjectileID.Bunny;

            int width = 24; int height = 20;
            Projectile.Size = new Vector2(width, height);

            DrawOffsetX -= 10;
            DrawOriginOffsetY -= 16;
        }

        public override bool TileCollideStyle (ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
            Player player = Main.player [Projectile.owner];
            float playerDistance = (player.Center - Projectile.Center).Length();
            fallThrough = (playerDistance > 250f);
            return true;
        }

        public override bool PreAI () {
            Main.player [Projectile.owner].bunny = false;
            return true;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.GuineaPig>()))
                Projectile.timeLeft = 2;
        }
    }
}