using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Buffs {
    public class Drunk : ModBuff {
        public override void SetStaticDefaults() {

            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.GetModPlayer<DrunkPlayer>().drunk = true;
            player.GetDamage(DamageClass.Generic) /= 2;
        }
    }

    internal class DrunkPlayer : ModPlayer {
        public bool drunk;

        public override void ResetEffects()
            => drunk = false;

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
            if (drunk)
                modifiers.FinalDamage *= 0.5f;
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
            if (drunk)
                modifiers.FinalDamage *= 0.5f;
        }
    }
}