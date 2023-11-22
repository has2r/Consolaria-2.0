using Consolaria.Common;
using Consolaria.Common.ModSystems;
using Consolaria.Content.Items.Summons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs.Bosses.Lepus {
    public class DisasterBunny : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 7;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            BestiaryText = this.GetLocalization("Bestiary");
        }

        public override void SetDefaults () {
            int width = 35; int height = 28;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = 3;
            AIType = 47;
            AnimationType = 47;

            NPC.damage = 20;
            NPC.defense = 4;

            NPC.lifeMax = 70;
            NPC.knockBackResist = 0.5f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.noTileCollide = false;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.DisasterBunnyBanner>();

            if (!NPC.AnyNPCs(ModContent.NPCType<Lepus>()))
                NPC.value = Item.buyPrice(silver: 5);
        }

		public override void ApplyDifficultyAndPlayerScaling (int numPlayers, float balance, float bossAdjustment)
            => NPC.lifeMax = 105;

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
            });
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            if (NPC.life <= 0) {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-4, 5)), ModContent.Find<ModGore>("Consolaria/DBG1").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-4, 5)), ModContent.Find<ModGore>("Consolaria/DBG2").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-4, 5)), ModContent.Find<ModGore>("Consolaria/DBG3").Type);
            }
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            LepusDropCondition lepusDropCondition = new();
            IItemDropRule conditionalRule = new LeadingConditionRule(lepusDropCondition);
            conditionalRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SuspiciousLookingEgg>(), 4));
            //RabbitInvasionDropCondition lepusDropCondition2 = new();
            //IItemDropRule conditionalRule2 = new LeadingConditionRule(lepusDropCondition2);
            //conditionalRule2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<GoldenCarrot>(), 4));
            npcLoot.Add(conditionalRule);
            //npcLoot.Add(conditionalRule2);
        }

        public override float SpawnChance (NPCSpawnInfo spawnInfo) {
            float spawnChance = DownedBossSystem.downedLepus ? 0.01f : 0.035f;
            if (SeasonalEvents.configEnabled) return SeasonalEvents.IsEaster() ?
                    SpawnCondition.OverworldDaySlime.Chance * spawnChance : 0f;
            else return SpawnCondition.OverworldDaySlime.Chance * spawnChance;
        }
    }
}