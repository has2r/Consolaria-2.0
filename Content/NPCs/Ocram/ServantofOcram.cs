using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Ocram
{
    public class ServantofOcram : ModNPC
    {
        public int ParentIndex {
            get => (int)NPC.ai[0] - 1;
            set => NPC.ai[0] = value + 1;
        }

        public bool HasParent => ParentIndex > -1;

        public static int BodyType()
            => ModContent.NPCType<Ocram>();
        
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Servant of Ocram");
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults() {
            int width = 60; int height = width;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = 23;
            AnimationType = NPCID.ServantofCthulhu;

            NPC.lifeMax = 450;
            NPC.damage = 40;

            NPC.defense = 8;
            NPC.knockBackResist = 0f;

            NPC.width = 54;
            NPC.height = 54;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCHit18;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            NPC.lifeMax = 600;
            NPC.damage = 60;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            int associatedNPCType = BodyType();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("A minion protecting his boss from taking damage by sacrificing itself. If none are alive, the boss is exposed to damage.")
            });
        }

        private bool Despawn() {
            if (Main.netMode != NetmodeID.MultiplayerClient &&
                (!HasParent || !Main.npc[ParentIndex].active || Main.npc[ParentIndex].type != BodyType())) {
                NPC.active = false;
                NPC.life = 0;
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                return true;
            }
            return false;
        }

        public override void AI()
            => NPC.position += NPC.velocity * 1.1f;
        
        public override void HitEffect(int hitDirection, double damage) {
            for (int i = 0; i < 3; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, hitDirection, -1f, 0, default, 1f);
            
            if (NPC.life <= 0) {
                Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/Servant_Gore").Type, 1f);
                for (int j = 0; j < 12; j++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, hitDirection, -1f, 0, default, 1f);               
            }
        }

        public override void OnKill() {
            if (Main.rand.Next(2) == 0)
                Item.NewItem(null, (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Heart);  
        }
    }
}