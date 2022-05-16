using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Consolaria.Content.Projectiles.Enemies;

namespace Consolaria.Content.NPCs
{
    public class ShadowSlime : ModNPC
    {
        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = 2;

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned
                }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults() {
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

           // banner = NPC.type;
            //bannerItem = mod.ItemType("ShadowSlimeBanner");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
                new FlavorTextBestiaryInfoElement("Steeped in the power of world evil, these slimes can spread their excretas.")
            });
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            if (Main.rand.Next(4) == 0)
                target.AddBuff(BuffID.Darkness, 60 * 15);
        }

        public override void HitEffect(int hitDirection, double damage) {
            Player player = Main.player[NPC.target];
            Dust.NewDust(NPC.position, NPC.width, NPC.height, 109, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
            if (NPC.life <= 0) {
                Vector2 vector = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] == 0f) {
                    for (int i = 0; i < Main.rand.Next(3, 5); i++) {
                        Vector2 vector2 = new Vector2(i - 2, -4f);
                        vector2.X *= 1f + Main.rand.Next(-50, 51) * 0.005f;
                        vector2.Y *= 1f + Main.rand.Next(-50, 51) * 0.005f;
                        vector2.Normalize();
                        vector2 *= 4f + Main.rand.Next(-50, 51) * 0.01f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), vector.X, vector.Y, vector2.X, vector2.Y, ModContent.ProjectileType<ShadowGel>(), 40, 0f, Main.myPlayer, 0f, 0f);
                        NPC.localAI[0] = 30f;
                    }
                }

                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
                for (int i = 0; i < 20; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 109, 2.5f * hitDirection, -2.5f, 0, default, 1f);    
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            var slimeDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.CorruptSlime, false);
            foreach (var slimeDropRule in slimeDropRules)
                npcLoot.Add(slimeDropRule);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            int y = spawnInfo.SpawnTileY;
            return ((spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson)  && Main.hardMode && y < Main.rockLayer) ? SpawnCondition.Corruption.Chance * 0.33f : 0f;
        }
    }
}
