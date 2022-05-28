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
        //private bool _checkSpawn;
        //private int _changeRotation = -1;
        private int timer = 0;
        private float speed = 18f;

        public override void SetStaticDefaults()
            => DisplayName.SetDefault("Lepus Egg");
        
        public override void SetDefaults() {
            int width = 44; int height = 48;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = -1;

            NPC.damage = 0;
            NPC.defense = 3;

            NPC.lifeMax = 120;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.friendly = false;
            NPC.noTileCollide = false;

            NPC.alpha = 255;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                new FlavorTextBestiaryInfoElement("LepusHelper.cs")
            });
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
            => false;
        
        private float Magnitude(Vector2 mag)
            => (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);

        /*  public override void AI() {
              if (!Main.npc[(int)NPC.ai[0]].active) {
                  NPC.life = 0;
                  NPC.HitEffect(0, 10.0);
                  NPC.active = false;
              }
              if (NPC.alpha > 0) { NPC.alpha -= 5; }
              timer++;//will hatch after time
              NPC.direction = Main.npc[(int)NPC.ai[0]].direction;
              NPC.rotation = Vector2.UnitY.RotatedBy((double)(timer / 40f * 6.25f), default(Vector2)).Y * 0.2f;
              Vector2 moveTo = Main.npc[(int)NPC.ai[0]].Center + offset;
              moveTo.Y -= 40;
              if (NPC.direction < 0) moveTo.X -= 20;   
              else  moveTo.X += 20; 
              Vector2 move = moveTo - NPC.Center;
              float magnitude = Magnitude(move);
              if (magnitude > speed) move *= speed / magnitude;

              if (timer >= 600) {
                  NPC.velocity = Vector2.Zero;
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
                      ushort bossType = (ushort)ModContent.NPCType<Lepus>();
                      if (NPC.CountNPCS(bossType) < 3) NPC.Transform(bossType);                
                      else NPC.Transform(ModContent.NPCType<DisasterBunny>());                   
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
              else NPC.velocity = move; 
          }*/

        public override void AI() {
            if (!Main.npc[(int)NPC.ai[0]].active) {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
            }
            if (NPC.alpha > 0) { NPC.alpha -= 5; }
            timer++;//will hatch after time
            NPC.direction = Main.npc[(int)NPC.ai[0]].direction;
            NPC.rotation = Vector2.UnitY.RotatedBy((double)(timer / 40f * 6.28318548f), default(Vector2)).Y * 0.2f;
            Vector2 moveTo = Main.npc[(int)NPC.ai[0]].Center;
            moveTo.Y -= 40;
            if (NPC.direction < 0) moveTo.X -= 20;      
            else  moveTo.X += 20; 
            Vector2 move = moveTo - NPC.Center;
            float magnitude = Magnitude(move);
            if (magnitude > speed) move *= speed / magnitude;
         
            if (timer >= 600) {
                NPC.velocity = Vector2.Zero;
                int gore1 = ModContent.Find<ModGore>("Consolaria/EggShellBig").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                }
                ushort bossType = (ushort)ModContent.NPCType<Lepus>();
                if (NPC.CountNPCS(bossType) < 3) NPC.Transform(bossType);
                else NPC.Transform(ModContent.NPCType<DisasterBunny>());
            }
            else  NPC.velocity = move; 
        }
        public override void HitEffect(int hitDirection, double damage) {
            if (NPC.life <= 0) {
                int _gore = ModContent.Find<ModGore>("Consolaria/EggShellBig").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), _gore);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), _gore);
                }
                SoundEngine.PlaySound(SoundID.Zombie10, NPC.position);
            }
        }
    }
}