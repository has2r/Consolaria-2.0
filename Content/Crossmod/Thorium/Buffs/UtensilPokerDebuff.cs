using Consolaria.Content.Crossmod.Thorium.Weapons;

using Terraria;
using Terraria.ModLoader;

using static Consolaria.Content.Crossmod.Thorium.Weapons.UtensilPoker;

namespace Consolaria.Content.Crossmod.Thorium.Buffs;

public sealed class UtensilPokerDebuff : ThoriumBuff_Base {
    public override void Update(NPC npc, ref int buffIndex) {
        npc.GetGlobalNPC<UtensilPokerDebuff_Handler>().IsEffectActive = true;
    }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public sealed class UtensilPokerDebuff_Handler : GlobalNPC {
    public bool IsEffectActive;

    public override bool InstancePerEntity => true;

    public override void ResetEffects(NPC npc) {
        IsEffectActive = false;
    }

    public override void UpdateLifeRegen(NPC npc, ref int damage) {
        if (!IsEffectActive) {
            return;
        }

        if (npc.lifeRegen > 0)
            npc.lifeRegen = 0;

        int num2 = 0;
        int num3 = 1;
        for (int i = 0; i < 1000; i++) {
            if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<UtensilPoker.UtensilPoker_Fork>() && (Main.projectile[i].ModProjectile as UtensilPoker_Fork).IsStickingToTarget == 1f && (Main.projectile[i].ModProjectile as UtensilPoker_Fork).TargetWhoAmI == (float)npc.whoAmI)
                num2++;
        }

        npc.lifeRegen -= num2 * 2 * 3;
        if (damage < num2 * 3 / num3)
            damage = num2 * 3 / num3;
    }
}
