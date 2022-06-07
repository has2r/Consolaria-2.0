using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs
{
	public class Drunk : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Drunk");
			Description.SetDefault("Halves all damage taken and dealt.");

			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<DrunkPlayer>().drunk = true;
			player.GetDamage(DamageClass.Generic) /= 2;
		}
	}

	internal class DrunkPlayer : ModPlayer
	{
		public bool drunk;

		public override void ResetEffects()
			=> drunk = false;

		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
			=> WhenDrunk(damage);

		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
			=> WhenDrunk(damage);

        private void WhenDrunk(int damage) {
			if (drunk)
				damage /= 2;
		}
    }
}