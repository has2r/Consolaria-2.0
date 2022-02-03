using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.Projectiles.Friendly
{
	public class EternalScythe : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.DemonScythe);
			AIType = ProjectileID.DemonScythe;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.scale = 0.65f;
			Projectile.penetrate = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
			=> target.AddBuff(BuffID.ShadowFlame, 180);

		public override void OnHitPvp(Player target, int damage, bool crit)
			=> target.AddBuff(BuffID.ShadowFlame, 180);

		public override Color? GetAlpha(Color lightColor)
			=> Color.White;
	}
}