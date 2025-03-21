using Consolaria.Common;
using Consolaria.Content.Items.Vanity;
using Consolaria.Content.Projectiles.Enemies;

using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
    public class ArchDemon : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        private float aiTimer;

        public override void SetStaticDefaults() {
            Main.npcFrameCount[NPC.type] = 5;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            BestiaryText = this.GetLocalization("Bestiary");
        }

        public override void SetDefaults() {
            int width = 90; int height = 50;
            NPC.Size = new Vector2(width, height);

            NPC.damage = 42;
            NPC.defense = 8;
            NPC.lifeMax = 300;

            NPC.value = Item.buyPrice(silver: 10);
            NPC.knockBackResist = 0.8f;
            NPC.rarity = 1;
            NPC.noGravity = true;

            NPC.aiStyle = 14;
            AnimationType = NPCID.Demon;

            NPC.lavaImmune = true;

            NPC.HitSound = SoundID.NPCHit21;
            NPC.DeathSound = SoundID.NPCDeath24;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.ArchDemonBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
            });
        }

        public override void AI() {
            Player player = Main.player[NPC.target];

            aiTimer++;
            NPC.TargetClosest();

            if (aiTimer >= 25f && aiTimer <= 95f) NPC.dontTakeDamage = false;
            else {
                if (Main.netMode != NetmodeID.Server) {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X + NPC.width * 0.5f + 35, NPC.position.Y + NPC.height * 0.5f), 2, 2, DustID.Torch, 0, -1f, 0, default, 1.75f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale *= 0.95f;
                    int dust2 = Dust.NewDust(new Vector2(NPC.position.X + NPC.width * 0.5f - 35, NPC.position.Y + NPC.height * 0.5f), 2, 2, DustID.Torch, 0, -1f, 0, default, 1.75f);
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].scale *= 0.95f;
                    int dust3 = Dust.NewDust(new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f + 35), 2, 2, DustID.Torch, 0, -1f, 0, default, 1.75f);
                    Main.dust[dust3].noGravity = true;
                    Main.dust[dust3].scale *= 0.95f;
                    int dust4 = Dust.NewDust(new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f - 35), 2, 2, DustID.Torch, 0, -1f, 0, default, 1.75f);
                    Main.dust[dust4].noGravity = true;
                    Main.dust[dust4].scale *= 0.95f;
                }
                NPC.dontTakeDamage = true;
            }

            if (aiTimer == 30f || aiTimer == 50f || aiTimer == 70f || aiTimer == 90f) {
                if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height)) {
                    float vel_ = 0.2f;
                    Vector2 shootVector = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                    float velX = Main.player[NPC.target].position.X + player.width * 0.5f - shootVector.X + Main.rand.Next(-100, 101);
                    float velY = Main.player[NPC.target].position.Y + player.height * 0.5f - shootVector.Y + Main.rand.Next(-100, 101);
                    float vel2_ = (float)Math.Sqrt((velX * velX + velY * velY));
                    vel2_ = vel_ / vel2_;
                    velX *= vel2_;
                    velY *= vel2_;
                    ushort pro = (ushort)Projectile.NewProjectile(NPC.GetSource_FromAI(), shootVector.X, shootVector.Y, velX, velY, ModContent.ProjectileType<ArchScythe>(), (int)(NPC.damage * 0.75f), 0f, player.whoAmI);
                    Main.projectile[pro].timeLeft = 300;
                    SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                }
            }
            else if (aiTimer % 100 == 0) {
                int randomIdleSound = Main.rand.Next(0, 3);
                if (randomIdleSound == 0) SoundEngine.PlaySound(SoundID.Zombie26, NPC.position);
                if (randomIdleSound == 1) SoundEngine.PlaySound(SoundID.Zombie27, NPC.position);
                if (randomIdleSound == 2) SoundEngine.PlaySound(SoundID.Zombie28, NPC.position);
                if (randomIdleSound == 3) SoundEngine.PlaySound(SoundID.Zombie29, NPC.position);
            }
            else if (aiTimer >= (300 + Main.rand.Next(300))) aiTimer = 0;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
            => target.AddBuff(BuffID.OnFire, 180);

        public override void HitEffect(NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.8f);
            if (NPC.life <= 0) {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/ArchdemonGore1").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/ArchdemonGore2").Type);
                for (int i = 0; i < 20; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 1f);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArchDemonMask>(), 15));

            DrunkWorldCondition drunkWorldCondition = new DrunkWorldCondition();
            npcLoot.Add(ItemDropRule.ByCondition(drunkWorldCondition, ItemID.CrystalShard, 1, 4, 9));
            npcLoot.Add(ItemDropRule.ByCondition(drunkWorldCondition, ItemID.CrystalStorm, 400));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if (!NPC.downedBoss3 && Main.zenithWorld || !NPC.downedBoss3 && Main.remixWorld)
                return 0f;
            return SpawnCondition.Underworld.Chance * 0.002f;
        }
    }
}