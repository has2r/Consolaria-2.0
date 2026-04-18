using Consolaria.Content.Items.Pets;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs;

public sealed class GiantAlbinoSwarmer : ModNPC {
    public static LocalizedText BestiaryText { get; private set; }

    public override void Load() {
        On_NPC.NewNPC += On_NPC_NewNPC;
    }

    private int On_NPC_NewNPC(On_NPC.orig_NewNPC orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target) {
        if (Type == NPCID.GiantFlyingAntlion && Main.rand.NextBool(20)) {
            Type = ModContent.NPCType<GiantAlbinoSwarmer>();
        }

        return orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
    }

    public override void SetStaticDefaults() {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GiantFlyingAntlion];

        NPCID.Sets.NPCBestiaryDrawModifiers value = new() {
            Position = new Vector2(6f, 0f),
            PortraitPositionXOverride = -10f,
            PortraitPositionYOverride = -20f
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

    //public override void AI() {
    //    if (!(NPC.shimmerTransparency > 0f)) {
    //        if (Main.rand.Next(1) == 0) {
    //            SoundEngine.PlaySound(Main.rand.NextFromList([SoundID.Zombie44, SoundID.Zombie45, SoundID.Zombie46]), NPC.Center);
    //        }
    //    }
    //}

    public override void HitEffect(NPC.HitInfo hit) {
        float num503 = 100f;
        float num504 = 50f;

        if (NPC.life > 0) {
            for (int num505 = 0; (double)num505 < hit.Damage / (double)NPC.lifeMax * (double)num503; num505++) {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 250, hit.HitDirection, -1f);
            }

            return;
        }

        for (int num506 = 0; (float)num506 < num504; num506++) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, 250, 2 * hit.HitDirection, -2f);
        }

        if (Main.dedServ) {
            return;
        }

        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoSwarmerGore1").Type);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoSwarmerGore2").Type);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoSwarmerGore3").Type);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoSwarmerGore4").Type);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoSwarmerGore4").Type);
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot) {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BleachCanister>(), 10));
        npcLoot.Add(ItemDropRule.Common(3772, 50));
        npcLoot.Add(ItemDropRule.Common(323, 3, 1, 2));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.AlbinoMandible>(), 25));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;
}
