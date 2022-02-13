using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
	public class Slime : ModProjectile
	{
        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }
        
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.Puppy);                      
            AIType = ProjectileID.Puppy;

            int width = 32; int height = 22;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].puppy = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Zombie>()))
                Projectile.timeLeft = 2;
        }

        public override void PostAI() {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8) {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;     
        }
    }
}
