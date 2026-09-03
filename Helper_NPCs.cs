using Terraria;
using Terraria.Audio;

namespace Consolaria;

public static class Helper_NPCs {
    public static void SetDefaultsToEnemy(this NPC npc, ushort lifeMax, 
                                                        ushort damage, 
                                                        ushort defense, 
                                                        float knockBackResist = 0f,
                                                        float spawnSlots = 0f, 
                                                        int aiStyle = -1,
                                                        bool boss = false) {
        npc.aiStyle = aiStyle;

        npc.lifeMax = lifeMax;
        npc.damage = damage;
        npc.defense = defense;
        npc.knockBackResist = knockBackResist;

        npc.friendly = false;

        npc.npcSlots = spawnSlots;

        npc.boss = boss;
    }

    public static void SetHitSounds(this NPC npc, SoundStyle? hitSound,
                                                  SoundStyle? deathSound) {
        npc.HitSound = hitSound;
        npc.DeathSound = deathSound;
    }

    public static void SetMiscellaneousProperties(this NPC npc, float dropCoins,
                                                                bool noGravity = true,
                                                                bool noTileCollide = true,
                                                                bool lavaImmune = false) {
        npc.value = dropCoins;

        npc.noGravity = noGravity;
        npc.noTileCollide = noTileCollide;

        npc.lavaImmune = lavaImmune;
    }

    public static void SetHitboxSizeValues(this NPC npc, ushort width, 
                                                         ushort height = 0) {
        npc.width = width;
        npc.height = height == 0 ? width : height;
    }

    public static void SetMaxFrames(this NPC npc, byte count) => Main.npcFrameCount[npc.type] = count;
}
