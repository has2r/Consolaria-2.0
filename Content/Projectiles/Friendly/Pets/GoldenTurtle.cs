using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
	public class GoldenTurtle : ModProjectile
	{
        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 13;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.CursedSapling);
            AIType = ProjectileID.CursedSapling;

            int width = 44; int height = 28;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].cSapling = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.GoldenTurtle>()))
                Projectile.timeLeft = 2;

            if (Projectile.velocity.Y == 0)
                Projectile.velocity = Projectile.velocity * 0.8f; 
        }

        public override void PostAI() {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 26) {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;
        }
    }
}
