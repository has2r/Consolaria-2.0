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
        private int timer = 0;

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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                new FlavorTextBestiaryInfoElement("LepusHelper.cs")
            });
        }

        public override void AI()
        {
            timer++;
            NPC.scale = (Main.mouseTextColor / 200f - 0.35f) * 0.46f + .8f;
            for (int i = NPC.oldPos.Length - 1; i > 0; i--)
                NPC.oldPos[i] = NPC.oldPos[i - 1];
            NPC.oldPos[0] = NPC.position;
            float h = 1f;
            if (timer >= 25 && h > 0)
            {
                h -= 0.05f;
                NPC.velocity *= h;
            }
            else NPC.rotation = NPC.velocity.X / 15f;

            if (timer >= 360) {             
                int gore1 = ModContent.Find<ModGore>("Consolaria/EggShell").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                    Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                }
                NPC.velocity.X = 0;
                NPC.Transform(ModContent.NPCType<DisasterBunny>());
            }
        }
        

        public override void HitEffect(int hitDirection, double damage) {
            if (NPC.life <= 0) {
                int gore1 = ModContent.Find<ModGore>("Consolaria/EggShell").Type;
                for (int i = 0; i < 1; i++) {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore1);
                }
                SoundEngine.PlaySound(SoundID.Zombie10, NPC.position);
            }
        }
    }
}