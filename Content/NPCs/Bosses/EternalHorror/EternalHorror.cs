using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    private static string EERIEOCRAM_MUSICPATH => "Assets/Music/EerieOcram";

    public static ushort SelfType => (ushort)ModContent.NPCType<EternalHorror>();

    public EternalHorror Self => NPC.As<EternalHorror>();

    public override void Load() {
        Load_BackgroundHooks();

        if (!Main.dedServ) {
            Load_Textures();
        }
    }

    public override void Unload() {
        Unload_Caches();
    }

    private partial void Unload_Caches();

    private partial void Load_BackgroundHooks();

    private partial void Load_Textures();

    public override void SetStaticDefaults() {
        NPC.SetMaxFrames(count: 6);
        NPC.SetTrail(length: 20);

        SetMiscellaneousProperties();

        SetDebuffImmuneData();

        SetBestiaryInfo();
    }

    private partial void SetBestiaryInfo();

    private void SetMiscellaneousProperties() {
        NPCID.Sets.MPAllowedEnemies[Type] = true;
        NPCID.Sets.MustAlwaysDraw[Type] = true;
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

        SetMusic();
    }

    private void SetMusic() {
        Music = MusicLoader.GetMusicSlot(Mod, EERIEOCRAM_MUSICPATH);
    }
}
