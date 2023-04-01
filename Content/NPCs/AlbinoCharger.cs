using Consolaria.Content.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
    public class AlbinoCharger : ModNPC {
        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 6;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults () {
            int width = 28; int height = 28;
            NPC.Size = new Vector2(width, height);

            NPC.damage = 30;
            NPC.lifeMax = 80;

            NPC.defense = 10;
            NPC.knockBackResist = 0.5f;

            NPC.HitSound = SoundID.NPCHit31;
            NPC.DeathSound = SoundID.NPCDeath34;

            NPC.value = Item.buyPrice(silver: 1);
            NPC.npcSlots = 0.8f;

            NPC.aiStyle = 3;
            AIType = NPCID.WalkingAntlion;
            AnimationType = NPCID.WalkingAntlion;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.AlbinoChargerBanner>();
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement("Rarely some antlion soldiers mutate, losing their coloring and becoming even more ferocious than their brothers-in-swarm.")
            });
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
            if (NPC.life <= 0) {
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand, 2.5f * hitDirection, -2.5f, 0, Color.White, 1f);

                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoGore4").Type, 1f);
            }
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AlbinoMandible>(), 30));
            var antlionsDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.WalkingAntlion, true);
            foreach (var antlionsDropRule in antlionsDropRules)
                npcLoot.Add(antlionsDropRule);
        }

        public override float SpawnChance (NPCSpawnInfo spawnInfo)
            => SpawnCondition.DesertCave.Chance * 0.025f;
    }
}