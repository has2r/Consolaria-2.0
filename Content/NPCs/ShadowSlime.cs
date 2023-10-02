using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Consolaria.Content.Items.Pets;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace Consolaria.Content.NPCs {
    public class ShadowSlime : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 2;

            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Poisoned] = true;

            BestiaryText = this.GetLocalization("Bestiary");
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
                new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
            });
        }

        public override void OnHitPlayer (Player target, Player.HurtInfo hurtInfo) {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Blackout, 60 * Main.rand.Next(4, 8));
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Demonite, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
            if (NPC.life <= 0) {
                for (int i = 0; i < 20; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Demonite, 2.5f * hit.HitDirection, -2.5f, 0, default, 1f);
            }
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PetriDish>(), 20));
            var slimeDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.CorruptSlime, false);
            foreach (var slimeDropRule in slimeDropRules)
                npcLoot.Add(slimeDropRule);
        }

        public override float SpawnChance (NPCSpawnInfo spawnInfo)
            => (spawnInfo.Player.ZoneCorrupt && Main.hardMode && spawnInfo.SpawnTileY < Main.rockLayer) ?
            SpawnCondition.Corruption.Chance * 0.005f : 0f;
    }
}