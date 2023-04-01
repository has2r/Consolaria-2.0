using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
    public class SpectralMummy : ModNPC {
        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 16;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Velocity = 0.85f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults () {
            int width = 40; int height = width;
            NPC.Size = new Vector2(width, height);

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;

            NPC.damage = 60;
            NPC.defense = 15;

            NPC.lifeMax = 250;
            NPC.knockBackResist = 0.1f;

            NPC.value = Item.buyPrice(silver: 10);

            NPC.noGravity = false;
            NPC.lavaImmune = false;

            NPC.aiStyle = 3;
            AIType = NPCID.DarkMummy;
            AnimationType = NPCID.DarkMummy;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.SpectralMummyBanner>();
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.HallowDesert,
                new FlavorTextBestiaryInfoElement("The overflowing light energy of these mummies made their tattered bodies highly unstable.")
            });
        }

        public override void OnHitPlayer (Player target, Player.HurtInfo hurtInfo) {
            if (Main.rand.NextBool(6))
                target.AddBuff(BuffID.Confused, 60 * 5);
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BlueFairy, 2.5f * hitDirection, -2.5f, 100, default, 1f);
            if (NPC.life <= 0) {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 11, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 12, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 13, 1f);
                for (int i = 0; i < 20; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BlueFairy, 2.5f * hitDirection, -2.5f, 100, default, 1f);
            }
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            var mummysDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.LightMummy, false);
            foreach (var mummyDropRule in mummysDropRules)
                npcLoot.Add(mummyDropRule);
        }

        public override float SpawnChance (NPCSpawnInfo spawnInfo)
            => (spawnInfo.Player.ZoneHallow && spawnInfo.Player.ZoneDesert && Main.hardMode) ?
            SpawnCondition.LightMummy.Chance * 0.005f : 0f;
    }
}