using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Lepus
{
    public class BigEgg : ModNPC
    {
        private bool _checkSpawn;
        private int _changeRotation = -1; 

        public override void SetStaticDefaults()
            => DisplayName.SetDefault("Lepus Egg");
        
        public override void SetDefaults() {
            int width = 44; int height = 48;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = 0;

            NPC.damage = 0;
            NPC.defense = 3;

            NPC.lifeMax = 90;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = false;
            NPC.noTileCollide = false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("LepusHelper.cs")
            });
        }

        public override void AI() {
            NPC.TargetClosest(true);
            if (!_checkSpawn) {
                Vector2 vector7 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));
                float rotation0 = (float)Math.Atan2((vector7.Y) - (Main.player[NPC.target].oldPosition.Y - (Main.player[NPC.target].height * 0.5f)), (vector7.X) - (Main.player[NPC.target].oldPosition.X + (Main.player[NPC.target].width * 0.5f)));
                NPC.velocity.X = (float)(Math.Cos(rotation0) * 12) * -1;
                NPC.velocity.Y = (float)(Math.Sin(rotation0) * 12) * -1;
                _checkSpawn = true;
            }

            NPC.localAI[0]++;
            if (NPC.localAI[0] >= 450) {
                int gore1 = ModContent.Find<ModGore>("Consolaria/EggShellBig").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                    Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                }
                NPC.Transform(ModContent.NPCType<Lepus>());
            }

            NPC.localAI[1]++;
            if (NPC.localAI[1] >= 80) {
                if (NPC.localAI[1] % 4 == 0) {
                    SoundEngine.PlaySound(3, (int)NPC.position.X, (int)NPC.position.Y, 11, 1f, -0.6f);
                    _changeRotation *= -1;
                }
                if (NPC.localAI[1] >= 100) {
                    NPC.localAI[1] = 0;
                    NPC.rotation = 0;
                }
                else NPC.rotation += 0.08f * _changeRotation;
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            if (NPC.life <= 0) {
                int _gore = ModContent.Find<ModGore>("Consolaria/EggShellBig").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), _gore);
                    Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), _gore);
                }
                SoundEngine.PlaySound(2, (int)NPC.position.X, (int)NPC.position.Y, 10, 1f, 0f);
            }
        }
    }
}