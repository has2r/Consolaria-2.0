using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
    public class GiantAlbinoCharger : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 6;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            BestiaryText = this.GetLocalization("Bestiary");
        }

        public override void SetDefaults () {
            int width = 50; int height = 30;
            NPC.Size = new Vector2(width, height);

            NPC.damage = 40;
            NPC.lifeMax = 120;

            NPC.defense = 16;
            NPC.knockBackResist = 0.6f;
            NPC.rarity = 1;

            NPC.HitSound = SoundID.NPCHit31;
            NPC.DeathSound = SoundID.NPCDeath34;

            NPC.value = Item.buyPrice(silver: 2);

            NPC.aiStyle = 3;
            AIType = NPCID.WalkingAntlion;
            AnimationType = NPCID.WalkingAntlion;

            Banner = ModContent.NPCType<AlbinoCharger>();
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.AlbinoChargerBanner>();
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
            });
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand, 2.5f * hit.HitDirection, -2.5f, 0, Color.White, 0.7f);
            if (NPC.life <= 0) {
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand, 2.5f * hit.HitDirection, -2.5f, 0, Color.White, 1f);

                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoGore3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/GiantAlbinoGore4").Type, 1f);
            }
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.AlbinoMandible>(), 30));
            npcLoot.Add(ItemDropRule.Common(ItemID.AntlionMandible, 3, 1, 2));
        }

        public override float SpawnChance (NPCSpawnInfo spawnInfo)
            => SpawnCondition.DesertCave.Chance * 0.015f;
    }
}