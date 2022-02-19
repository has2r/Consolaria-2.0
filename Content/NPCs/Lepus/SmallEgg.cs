using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Lepus
{
    public class SmallEgg : ModNPC
    {
        private bool _checkSpawn;

        public override void SetStaticDefaults() 
            => DisplayName.SetDefault("Lepus Egg");
        

        public override void SetDefaults()
        {
            int width = 22; int height = 24;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = 0;

            NPC.damage = 0;
            NPC.defense = 3;

            NPC.lifeMax = 65;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.noTileCollide = false;
            NPC.friendly = false;
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
            if (NPC.localAI[0] >= 360) {
                int gore1 = ModContent.Find<ModGore>("Consolaria/EggShell").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                    Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                }
                NPC.Transform(ModContent.NPCType<DisasterBunny>());
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            if (NPC.life <= 0) {
                int gore1 = ModContent.Find<ModGore>("Consolaria/EggShell").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                    Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                }
                SoundEngine.PlaySound(2, (int)NPC.position.X, (int)NPC.position.Y, 10, 1f, 0f);
            }
        }
    }
}