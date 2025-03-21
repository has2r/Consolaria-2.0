using Consolaria.Content.NPCs.Bosses.Ocram;
using Consolaria.Content.Projectiles.Enemies;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Consolaria.Common {
    public class ConsolariaPlayer : ModPlayer {
        private bool killedByOcram;

        public override void OnRespawn ()
            => killedByOcram = false;

        public override void OnHitByNPC (NPC npc, Player.HurtInfo hurtInfo) {
            if (npc.type == ModContent.NPCType<Ocram>() && JustDied(hurtInfo.Damage))
                killedByOcram = true;
        }

        public override void OnHitByProjectile (Projectile proj, Player.HurtInfo hurtInfo) {
            if ((proj.type == ModContent.ProjectileType<OcramLaser1>() || proj.type == ModContent.ProjectileType<OcramLaser2>() || proj.type == ModContent.ProjectileType<OcramScythe>() || proj.type == ModContent.ProjectileType<OcramSkull>()) && JustDied(hurtInfo.Damage))
                killedByOcram = true;
        }

        public override bool PreKill (double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if (killedByOcram && Main.rand.NextBool(5))
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was sent to Red's House by Ocram");
            return true;
        }

        private bool JustDied (int damage) => damage >= Player.statLife;
    }
}