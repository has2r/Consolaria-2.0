using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class OcramEyeGlow : ModProjectile {
        public override void SetDefaults() {
            int width = 10; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.tileCollide = false;

            Projectile.scale = 1.5f;
            Projectile.alpha = 100;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
            Projectile.Center = playerCenter + Projectile.velocity * 15f;

            Projectile.alpha += 20;
            if (Projectile.alpha >= 255) Projectile.active = false;
        }

        public override Color? GetAlpha(Color lightColor)
            => new Color(Color.BlueViolet.R * (255 - Projectile.alpha) / 255, Color.BlueViolet.G * (255 - Projectile.alpha) / 255, Color.BlueViolet.B * (255 - Projectile.alpha) / 255, 0);

        public override bool? CanDamage()
            => false;
        public override bool? CanCutTiles()
            => false;
    }
}