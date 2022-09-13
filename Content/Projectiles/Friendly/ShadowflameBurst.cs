using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class ShadowflameBurst : ModProjectile {
        //public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetStaticDefaults() => Main.projFrames[Projectile.type] = 6;

        private readonly int lifeLimit = 40;
        private float rot;
        private float scal;

        public override void SetDefaults () {
            int width = 8; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Melee;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = lifeLimit;

            Projectile.extraUpdates = 1;
        }

        public override void AI () {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.15f) / 255f, ((255 - Projectile.alpha) * 0.45f) / 255f, ((255 - Projectile.alpha) * 0.05f) / 255f);
            if (Projectile.timeLeft > lifeLimit) 
                Projectile.timeLeft = lifeLimit;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.Kill();

            if (Projectile.ai [0] > 5f && Projectile.timeLeft > 15) {
                if (Main.netMode != NetmodeID.Server) {
                    if (Main.rand.NextBool(10)) {
                        int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f);
                        Main.dust [dust].noGravity = true;
                        Main.dust [dust].velocity *= 2.5f;
                        Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                        Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 130, default, 1.5f);
                    }
                }
            }
            else Projectile.ai [0] += 1f;
            return;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, frameHeight * 0.5f);
            Vector2 drawPos = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Rectangle frameRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Color color = new Color(60 + Projectile.timeLeft * 4, 40 + Projectile.timeLeft * 4, 90 + Projectile.timeLeft * 4, 50);
            rot += 0.1f;
            if (rot >= Math.PI * 2) rot -= (float)Math.PI * 2;
            if (scal < 3f) scal += 0.1f;
            if (Projectile.ai[1] == 2) spriteBatch.Draw(texture, drawPos, frameRect, color, rot, drawOrigin, scal, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 300);

        public override void OnHitPvp (Player target, int damage, bool crit)
            => target.AddBuff(BuffID.ShadowFlame, 300);

        public override bool OnTileCollide (Vector2 oldVelocity) {
            Projectile.Kill();
            return false;
        }
    }
}