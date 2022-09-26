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
            Player player = Main.player [Projectile.owner];
            int evilBunny = WorldGen.crimson ? NPCID.CrimsonBunny : NPCID.CorruptBunny;
            if (Main.rand.NextBool(200)) NPC.NewNPC(Projectile.GetSource_Death(), (int) Projectile.Center.X, (int) Projectile.Center.Y, NPCID.Bunny);
            if (Main.rand.NextBool(200)) NPC.NewNPC(Projectile.GetSource_Death(), (int) Projectile.Center.X, (int) Projectile.Center.Y, NPCID.Bird);
            if (Main.rand.NextBool(300)) NPC.NewNPC(Projectile.GetSource_Death(), (int) Projectile.Center.X, (int) Projectile.Center.Y, evilBunny);
            int trollingChance = player.name == "has2r" ? 50 : 600;
            if (Main.rand.NextBool(trollingChance)) NPC.NewNPC(Projectile.GetSource_Death(), (int) Projectile.Center.X, (int) Projectile.Center.Y, NPCID.ExplosiveBunny);

            if (Main.netMode != NetmodeID.Server) {
                int easterEggGoreType = ModContent.Find<ModGore>("Consolaria/EasterEggGore").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, new Vector2(Main.rand.Next(-2, 2), -1), easterEggGoreType);
                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, new Vector2(Main.rand.Next(-2, 2), -1), easterEggGoreType);
                }
            }
            SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/EggCrack") { Volume = 0.4f, Pitch = 0.1f, MaxInstances = 0 }, Projectile.position);
        }
    }
}