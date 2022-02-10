using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
	public class Zombie : ModProjectile
	{
        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 3;
            Main.projPet[Projectile.type] = true;
        }
        
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.Penguin);                      
            AIType = ProjectileID.Penguin;

            int width = 34; int height = 44;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].babyFaceMonster = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Zombie>()))
                Projectile.timeLeft = 2;

            if (Projectile.localAI[0] >= 800f)Projectile.localAI[0] = 0f;
            
            if (Vector2.Distance(player.Center, Projectile.Center) > 500f) {
                Projectile.position.X = player.position.X;
                Projectile.position.Y = player.position.Y;
            }
        }

        public override void PostAI() {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 18) {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;     
        }
    }
}
