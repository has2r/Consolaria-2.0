using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Lepus
{
    internal class SmallEgg : ModNPC
	{
        public ref float Timer
            => ref NPC.ai[0];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lepus Egg");
        }

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

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
                new FlavorTextBestiaryInfoElement("Lepus Egg")
            });
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
            => false;

        public override void AI()
        {
            float maxRotation = 0.3f;
            int max = 2000;
            float current = (float)Timer / (float)max;
            Timer += 1f * ((current + 0.5f) * 5f);
            float speed = current < 0.5f ? current : 1f - current;
            NPC.rotation = MathHelper.Lerp(-maxRotation, maxRotation, speed);
            NPC.scale = (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f;
            NPC.velocity *= 0.95f;
            if (++Timer >= max || (Collision.SolidCollision(NPC.Center, 10, 10) && NPC.oldVelocity.Length() > 5f))
            {
                NPC.life = -1;
                NPC.HitEffect();
                NPC.active = false;
                if (Main.netMode == NetmodeID.Server)
                {
                    NPC.netSkip = -1;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
                    NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 2, (int)NPC.Center.X, (int)NPC.Center.Y, NPC.type);
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int max = 2000;
            Death(Timer >= max);
        }

        private void Death(bool spawnBunny = false)
		{
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            if (NPC.life <= 0)
            {
                int gore = ModContent.Find<ModGore>("Consolaria/EggShell").Type;
                var entitySource = NPC.GetSource_Death();
                for (int i = 0; i < 2; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), gore);
                }
                if (spawnBunny)
                {
                    int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DisasterBunny>());
                    if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, number: index);
                    }
                }
            }
        }
    }
}