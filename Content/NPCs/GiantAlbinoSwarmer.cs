using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs;

public sealed class GiantAlbinoSwarmer : ModNPC {
    public static LocalizedText BestiaryText { get; private set; }

    public override void SetStaticDefaults() {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GiantFlyingAntlion];

        NPCID.Sets.NPCBestiaryDrawModifiers value = new() {
            Velocity = 1f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

        BestiaryText = this.GetLocalization("Bestiary");
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        bestiaryEntry.Info.AddRange([BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                                     new FlavorTextBestiaryInfoElement(BestiaryText.ToString())]);
    }

    public override void SetDefaults() {
        NPC.CloneDefaults(NPCID.GiantFlyingAntlion);

        AIType = NPCID.GiantFlyingAntlion;
        AnimationType = NPCID.GiantFlyingAntlion;

        NPC.rarity = 1;

        Banner = ModContent.NPCType<AlbinoSwarmer>();

        float scale = 1.25f;
        NPC.damage = (int)((float)NPC.damage * scale);
        NPC.defense = (int)((float)NPC.defense * scale);
        NPC.lifeMax = (int)((float)NPC.lifeMax * scale);
        NPC.value = (int)(NPC.value * scale);
    }

    public override void HitEffect(NPC.HitInfo hit) {

    }

    public override void ModifyNPCLoot(NPCLoot npcLoot) {

    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;
}
