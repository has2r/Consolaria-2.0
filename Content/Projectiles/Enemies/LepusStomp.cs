using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Enemies
{
    public class LepusStomp : ModProjectile
    {
        public override string Texture => "Consolaria/Assets/Textures/Empty";

        public override void SetStaticDefaults()
            => DisplayName.SetDefault("Lepus Stomp");

        public override void SetDefaults() {
            Projectile.CloneDefaults(683);
            AIType = 683;
            Projectile.width = 100;
            Projectile.height = 20;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 500;
        }
    }
}