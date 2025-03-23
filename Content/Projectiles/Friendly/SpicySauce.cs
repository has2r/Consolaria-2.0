using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class SpicySauce : ModProjectile {
        public override void SetDefaults() {
            int width = 30; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = 2;

            Projectile.friendly = true;
            Projectile.tileCollide = true;

            Projectile.penetrate = 1;
        }

        public override void OnKill(int timeLeft) {
            float _distance = 90f;
            for (int _findPlayer = 0; _findPlayer < byte.MaxValue; _findPlayer++) {
                Player player = Main.player[_findPlayer];
                if (player.active && !player.dead && Vector2.Distance(Projectile.Center, player.Center) < _distance)
                    player.AddBuff(BuffID.Oiled, 300, false);
            }
            for (int _findNPC = 0; _findNPC < Main.maxNPCs; _findNPC++) {
                NPC npc = Main.npc[_findNPC];
                if (npc.active && npc.life > 0 && Vector2.Distance(Projectile.Center, npc.Center) < _distance)
                    npc.AddBuff(BuffID.Oiled, 300);
            }

            SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
            if (Main.netMode != NetmodeID.Server) {
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, -Projectile.oldVelocity * 0.2f, 704, 1f);
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, -Projectile.oldVelocity * 0.2f, 705, 1f);
            }
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0, Projectile.velocity.Y * 0, ModContent.ProjectileType<SpicyExplosion>(), (int)((double)Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner);
            --Projectile.penetrate;
        }
    }
}