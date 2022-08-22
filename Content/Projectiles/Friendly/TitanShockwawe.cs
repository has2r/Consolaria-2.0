using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly {
    public class TitanShockwawe : ModProjectile {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        private readonly int rippleSize = 1200;
        private readonly int rippleCount = 3;
        private readonly int rippleSpeed = 80;

        private float distortStrength = 400f;
        private float step = 0f;

        public override void SetDefaults () {
            int width = 200; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.aiStyle = -1;
            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 40;

            Projectile.tileCollide = false;
        }

        public override void AI () {
            Player player = Main.player [Projectile.owner];
            Projectile.Center = player.Center;

            step += 0.015f;
            distortStrength++;
            if (Main.netMode != NetmodeID.Server && !Filters.Scene ["Shockwave"].IsActive())
                Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(Projectile.Center);
            if (Main.netMode != NetmodeID.Server && Filters.Scene ["Shockwave"].IsActive())
                Filters.Scene ["Shockwave"].GetShader().UseProgress(step).UseOpacity(distortStrength);
        }

        public override void Kill (int timeLeft) {
            if (Main.netMode != NetmodeID.Server && Filters.Scene ["Shockwave"].IsActive())
                Filters.Scene ["Shockwave"].Deactivate();
        }

        public override bool? CanCutTiles () => false;
    }
}