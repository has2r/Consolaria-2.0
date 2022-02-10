using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets
{
	public class PetTurkey : ModProjectile
	{
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Turkey");
            Main.projFrames[Projectile.type] = 2;
            Main.projPet[Projectile.type] = true;
        }
        
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.Penguin);                      
            AIType = ProjectileID.Penguin;

            int width = 40; int height = 34;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI() {
            Main.player[Projectile.owner].penguin = false;
            return true;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.PetTurkey>()))
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
