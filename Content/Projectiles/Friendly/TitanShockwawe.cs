using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
    public class TitanShockwawe : ModProjectile
    {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        private int rippleSize = 450;
        private int rippleCount = 10;
        private int rippleSpeed = 50;
        private float distortStrength = 500f;
        private float step = 0f;

        public override void SetDefaults() {
            int width = 180; int height = width;
            Projectile.Size = new Vector2(width, height);

            Projectile.DamageType = DamageClass.Generic;

            Projectile.aiStyle = -1;
            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 45;

            Projectile.tileCollide = false;
        }

        public override void AI() {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            step += 0.01f;
            distortStrength++;
            if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive())
                Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(Projectile.Center);           
            if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                Filters.Scene["Shockwave"].GetShader().UseProgress(step).UseOpacity(distortStrength);      
        }

        public override void Kill(int timeLeft) {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                Filters.Scene["Shockwave"].Deactivate();      
        }

        public override bool? CanCutTiles() => false;
    }
}
