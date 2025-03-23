using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Lepus {
    internal class SmallEgg : ModNPC {
        public ref float Timer
            => ref NPC.ai[0];

        public override void SetStaticDefaults() {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
        }

        public override void SetDefaults() {
            int width = 22; int height = 24;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = 0;

            NPC.damage = 0;
            NPC.defense = 1;

            NPC.lifeMax = 30;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/EggCrack");

            NPC.noTileCollide = false;
            NPC.friendly = false;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
            => NPC.lifeMax = 50 + (int)(numPlayers > 1 ? NPC.lifeMax * 0.15 * numPlayers : 0);

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
            => false;

        public override void AI() {
            NPC.direction = 0;
            float maxRotation = 0.3f;
            int max = 2000;
            float current = (float)Timer / max;
            Timer += Main.rand.NextFloat(0f, 1f) * Main.rand.NextFloat(0f, 1f) * 3f * ((current + 0.5f) * 5f);
            float speed = current < 0.5f ? current : 1f - current;
            NPC.rotation = MathHelper.Lerp(-maxRotation, maxRotation, speed);
            NPC.scale = (Main.mouseTextColor / 200f - 0.35f) * 0.46f + 0.8f;
            NPC.velocity *= 0.95f;
            if (++Timer >= max || (Collision.SolidCollision(NPC.Center, 10, 10) && NPC.oldVelocity.Length() > 5f)) {
                NPC.life = -1;
                NPC.HitEffect();
                NPC.active = false;
                if (Main.netMode == NetmodeID.Server) {
                    NPC.netSkip = -1;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit) {
            int max = 2000;
            Death(Timer >= max);
        }

        private void Death(bool spawnBunny = false) {
            if (NPC.life <= 0) {
                if (Main.netMode != NetmodeID.Server) {
                    int gore = ModContent.Find<ModGore>("Consolaria/EggShell").Type;
                    var entitySource = NPC.GetSource_Death();
                    for (int i = 0; i < 2; i++) {
                        Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 2), 0), gore);
                    }
                }
                int type = ModContent.NPCType<DisasterBunny>();
                if (spawnBunny && NPC.CountNPCS(type) < (Main.expertMode ? 15 : 10) && NPC.CountNPCS(ModContent.NPCType<Lepus>()) > 0) {
                    if (Main.netMode != NetmodeID.Server) {
                        SoundStyle style = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/EggCrack") { Volume = 0.8f };
                        SoundEngine.PlaySound(style, NPC.Center);
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient) {
                        return;
                    }
                    int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, type);
                    if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs) {
                        NetMessage.SendData(MessageID.SyncNPC, number: index);
                    }
                }
            }
        }
    }
}