using Consolaria.Content.Items.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
    public class Orca : ModNPC {
        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 4;

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
                SpecificallyImmuneTo = new int [] {
                    BuffID.Confused
                }
            };

            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults () {
            int width = 120; int height = 50;
            NPC.Size = new Vector2(width, height);

            NPC.damage = 50;
            NPC.lifeMax = 400;

            NPC.defense = 10;
            NPC.knockBackResist = 0.1f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.value = Item.buyPrice(silver: 12);
            NPC.noGravity = true;

            NPC.aiStyle = 16;
            AIType = NPCID.Shark;
            AnimationType = NPCID.Shark;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.OrcaBanner>();
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("Since there are no whales to be seen in these oceans, these black'n'white predators will hunt anything else... including you!")
            });
        }

        public override void OnHitPlayer (Player target, Player.HurtInfo hurtInfo) {
            if (Main.rand.NextBool(2))
                target.AddBuff(BuffID.Bleeding, 60 * 5);
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
            if (NPC.life <= 0) {
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 1f);

                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/Gore_490").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/Gore_491").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/Gore_492").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/Gore_493").Type, 1f);
            }
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            var sharksDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Shark, true);
            foreach (var sharkDropRule in sharksDropRules)
                npcLoot.Add(sharkDropRule);
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GoldenSeaweed>(), 20));
        }

        public override float SpawnChance (NPCSpawnInfo spawnInfo)
            => SpawnCondition.OceanMonster.Chance * 0.025f;
    }
}