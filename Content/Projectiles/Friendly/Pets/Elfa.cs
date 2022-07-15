using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly.Pets {
    public class Elfa : ModProjectile {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Elfa");

            Main.projFrames [Projectile.type] = 9;
            Main.projPet [Projectile.type] = true;
        }

        public override void SetDefaults () {
            Projectile.CloneDefaults(ProjectileID.Penguin);
            AIType = ProjectileID.Penguin;

            int width = 30; int height = 48;
            Projectile.Size = new Vector2(width, height);
        }

        public override bool PreAI () {
            Main.player [Projectile.owner].penguin = false;
            return true;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.Leprechaun>()))
                Projectile.timeLeft = 2;
        }
    }
}