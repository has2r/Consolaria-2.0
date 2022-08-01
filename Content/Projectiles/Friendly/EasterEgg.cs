using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class EasterEgg : ModProjectile {
        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.RottenEgg);
            Projectile.aiStyle = 1;

            int width = 14; int height = 16;
            Projectile.Size = new Vector2(width, height);

            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void Kill (int timeLeft) {
            SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/EggCrack") { Volume = 0.7f}, Projectile.position);

            if (Main.rand.NextBool(100)) NPC.NewNPC(Projectile.GetSource_Death(), (int) Projectile.Center.X, (int) Projectile.Center.Y, NPCID.Bunny);
            if (Main.rand.NextBool(100)) NPC.NewNPC(Projectile.GetSource_Death(), (int) Projectile.Center.X, (int) Projectile.Center.Y, NPCID.Bird);
            if (Main.rand.NextBool(150)) NPC.NewNPC(Projectile.GetSource_Death(), (int) Projectile.Center.X, (int) Projectile.Center.Y, NPCID.CorruptBunny);
            if (Main.rand.NextBool(150)) NPC.NewNPC(Projectile.GetSource_Death(), (int) Projectile.Center.X, (int) Projectile.Center.Y, NPCID.CrimsonBunny);

            int EasterEggGoreType = ModContent.Find<ModGore>("Consolaria/EasterEggGore").Type;
            for (int i = 0; i < 1; i++) {
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, new Vector2(Main.rand.Next(-2, 2), -1), EasterEggGoreType);
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, new Vector2(Main.rand.Next(-2, 2), -1), EasterEggGoreType);
            }
        }
    }
}