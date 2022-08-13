using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Consolaria.Content.Projectiles.Enemies;
using Consolaria.Content.Items.Pets;
using Terraria.GameContent.ItemDropRules;

namespace Consolaria.Content.NPCs
{
    public class ShadowSlime : ModNPC {
        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 2;

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
                SpecificallyImmuneTo = new int [] {
                    BuffID.Poisoned
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults () {
            int width = 40; int height = 30;
            NPC.Size = new Vector2(width, height);

            NPC.lifeMax = 125;
            NPC.defense = 20;

            NPC.damage = 20;
            NPC.knockBackResist = 0f;

            NPC.value = Item.buyPrice(silver: 5);
            NPC.alpha = 80;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            AnimationType = 81;
            NPC.aiStyle = 1;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.ShadowSlimeBanner>();
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement("Steeped in the power of Corruption, these slimes can spread their excretas.")
            });
        }

        public override void OnHitPlayer (Player target, int damage, bool crit) {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Blackout, 60 * Main.rand.Next(4, 8));
        }

        public override void HitEffect (int hitDirection, double damage) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Demonite, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
            if (NPC.life <= 0) {
                for (int i = 0; i < 20; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Demonite, 2.5f * hitDirection, -2.5f, 0, default, 1f);
            }
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetriDish>(), 10));
            var slimeDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.CorruptSlime, false);
            foreach (var slimeDropRule in slimeDropRules)
                npcLoot.Add(slimeDropRule);
        }

        public override float SpawnChance (NPCSpawnInfo spawnInfo)
            => (spawnInfo.Player.ZoneCorrupt && Main.hardMode && spawnInfo.SpawnTileY < Main.rockLayer) ?
            SpawnCondition.Corruption.Chance * 0.15f : 0f;
    }
}
