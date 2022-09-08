using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Ocram {
    public class ServantofOcram : ModNPC {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Servant of Ocram");
            Main.npcFrameCount [NPC.type] = 2;

            NPCID.Sets.DontDoHardmodeScaling [Type] = true;
            NPCID.Sets.CantTakeLunchMoney [Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults () {
            int width = 54; int height = width;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = 23;
            AnimationType = NPCID.ServantofCthulhu;

            NPC.lifeMax = 500;
            NPC.damage = 40;

            NPC.defense = 10;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCHit18;
        }

        public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
            NPC.lifeMax = 650 + (int)(numPlayers > 1 ? NPC.lifeMax * 0.1f * numPlayers : 0);
            NPC.damage = 60;
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            int associatedNPCType = ModContent.NPCType<Ocram>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds [associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Evil creatures manifested from Ocram, brought forth to protect their master by any means necessary.")
            });
        }

        public override void AI () {
            NPC.direction = 1;
            NPC.position += NPC.velocity * 1.1f;
        }

        public override bool PreDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new(texture.Width / 2, texture.Height / Main.npcFrameCount [NPC.type] / 2);
            Main.spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale), new Rectangle?(NPC.frame), Color.White, NPC.rotation, origin, NPC.scale, effects, 0f);
            for (int i = 1; i < NPC.oldPos.Length; i++) {
                Color color = Color.Red;
                color = NPC.GetAlpha(color);
                color *= (NPC.oldPos.Length - i) / 15f;
                Main.spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * i * 0.5f, new Rectangle?(NPC.frame), color, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
            return true;
        }

        public override void HitEffect (int hitDirection, double damage) {
            if (Main.netMode == NetmodeID.Server)
                return;

            for (int i = 0; i < 3; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, hitDirection, -1f, 0, default, 1f);
            if (NPC.life <= 0) {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * 0.75f, ModContent.Find<ModGore>("Consolaria/Servant_Gore").Type, 0.75f);
                for (int j = 0; j < 12; j++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, hitDirection, -1f, 0, default, 1f);
            }
        }

        public override void OnKill () {
            if (Main.rand.NextBool(2))
                Item.NewItem(NPC.GetSource_Death(), (int) NPC.position.X, (int) NPC.position.Y, NPC.width, NPC.height, ItemID.Heart);
        }
    }
}