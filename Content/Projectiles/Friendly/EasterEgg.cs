using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class EasterEgg : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.RottenEgg);
            Projectile.aiStyle = 1;

            int width = 14; int height = 16;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void Kill(int timeLeft) {
            if (Main.rand.Next(100) == 0) NPC.NewNPC(null,(int)Projectile.Center.X, (int)Projectile.Center.Y, NPCID.Bunny);
            if (Main.rand.Next(100) == 0) NPC.NewNPC(null,(int)Projectile.Center.X, (int)Projectile.Center.Y, NPCID.Bird);
            if (Main.rand.Next(150) == 0) NPC.NewNPC(null,(int)Projectile.Center.X, (int)Projectile.Center.Y, NPCID.CorruptBunny);        
            if (Main.rand.Next(150) == 0) NPC.NewNPC(null,(int)Projectile.Center.X, (int)Projectile.Center.Y, NPCID.CrimsonBunny);          
            
            int EasterEggGoreType = ModContent.Find<ModGore>("Consolaria/EasterEggGore").Type;
            for (int i = 0; i < 1; i++) {
                Gore.NewGore(Projectile.position, new Vector2(Main.rand.Next(1, 1), Main.rand.Next(1, 1)), EasterEggGoreType);
                Gore.NewGore(Projectile.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), EasterEggGoreType);
            }
        }
    }
}
