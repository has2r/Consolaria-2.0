using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    public static ushort SelfType => (ushort)ModContent.NPCType<EternalHorror>();

    public static Color MainPurpleColor => new(175, 85, 255);
    public static Color MainPurpleColor_Dynamic => Color.Lerp(new(175, 85, 255), Color.Lerp(new(198, 123, 173), new(131, 186, 64), 0.5f), Helper.Wave(0f, 1f, 1f, 0f));

    public override void Load() {
        Load_Background();
    }

    private partial void Load_Background();

    public override void SetStaticDefaults() {
        NPC.SetMaxFrames(count: 6);

        SetMiscellaneousProperties();

        SetDebuffImmuneData();

        SetBestiaryInfo();
    }

    private partial void SetBestiaryInfo();

    private void SetMiscellaneousProperties() {
        NPCID.Sets.MPAllowedEnemies[Type] = true;
    }

    private void SetDebuffImmuneData() {
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
    }

    public override void SetDefaults() {
        NPC.SetHitboxSizeValues(width: 314, height: 216);

        NPC.SetDefaultsToEnemy(lifeMax: 54000,
                               damage: 55, 
                               defense: 36, 
                               spawnSlots: 10f,
                               boss: true);

        NPC.SetHitSounds(hitSound: SoundID.NPCHit18,
                         deathSound: SoundID.NPCDeath18);

        NPC.SetMiscellaneousProperties(dropCoins: Item.buyPrice(gold: 15),
                                       lavaImmune: true);

        NPC.SpawnWithHigherTime(timeMult: 30);
    }
}
