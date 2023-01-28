using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class VulcanBlast : ModProjectile
    {
        private Vector2 direction;
        private float glowRotation;
        private float glowAlpha;
        private float randRotation = Main.rand.NextFloat() * (float)Math.PI / 2;

        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.tileCollide = false;

            Projectile.timeLeft = 30;
            Projectile.penetrate = -1;

            Projectile.rotation = Main.rand.NextFloat(0, (float)Math.PI * 2f);
            Projectile.alpha = 50;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 3;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft == 30) direction = new Vector2(0, -5).RotatedByRandom(Math.PI * 0.5f);
            if (Projectile.timeLeft % 7 == 0) {
                int fire = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - direction * 2f, direction, 85, Projectile.damage * 2, Projectile.knockBack, player.whoAmI);
                Main.projectile[fire].tileCollide = false;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            Texture2D glow = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Projectiles/LightTrail_1");
            Vector2 position = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition; // + new Vector2(glow.Width, glow.Height) / 2f
            Vector2 glowOrigin = new Vector2(glow.Width / 2, glow.Height / 2);

            glowRotation += 0.1f;
            if (glowRotation > Math.PI * 2f) glowRotation -= (float)Math.PI * 2f;

            if (Projectile.timeLeft > 20) glowAlpha += 0.1f;
            if (Projectile.timeLeft < 10) glowAlpha -= 0.1f;

            Color glowColor = Color.Orange * 1.8f;
            glowColor.A = 0;

            Color glowRed = Color.Red * 1.8f;
            glowRed.A = 0;

            spriteBatch.Draw(glow, position, null, glowRed * 0.5f * glowAlpha, randRotation, glowOrigin, Projectile.scale + glowAlpha, default, 0f);
            spriteBatch.Draw(glow, position, null, glowRed * 0.5f * glowAlpha, randRotation + (float)Math.PI * 0.5f, glowOrigin, Projectile.scale + glowAlpha, default, 0f);
            spriteBatch.Draw(glow, position, null, glowColor * glowAlpha, glowRotation, glowOrigin, Projectile.scale * 0.6f + glowAlpha, default, 0f);
            spriteBatch.Draw(glow, position, null, glowColor * glowAlpha, glowRotation + (float)Math.PI * 0.5f, glowOrigin, Projectile.scale * 0.6f + glowAlpha, default, 0f);

            return false;
        }

        public override bool? CanCutTiles()
            => false;
    }
}