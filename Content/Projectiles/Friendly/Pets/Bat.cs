using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
	public class Bat : ModProjectile
	{
        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 5;
            Main.projPet[Projectile.type] = true;
        }
        
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.ZephyrFish);                      
            AIType = ProjectileID.ZephyrFish;

            int width = 10; int height = width;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].zephyrfish = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Bat>()))
                Projectile.timeLeft = 2;    
        }

        public override void PostAI() {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 10) {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;     
        }
    }
}
