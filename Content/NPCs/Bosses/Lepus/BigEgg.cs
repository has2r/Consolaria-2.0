using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Lepus {
    internal class BigEgg : ModNPC {
        public ref float Timer
            => ref NPC.ai [0];

        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Lepus Egg");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
                SpecificallyImmuneTo = new int [] {
                    BuffID.Confused,
                    BuffID.Poisoned,
                    BuffID.Venom
                }
            };
        }

        public override void SetDefaults () {
            int width = 44; int height = 48;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = -1;

            NPC.damage = 0;
            NPC.defense = 3;

            NPC.lifeMax = 100;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/EggCrack");

            NPC.friendly = false;
            NPC.noTileCollide = false;
        }

        public override void ScaleExpertStats (int numPlayers, float bossLifeScale) 
            => NPC.lifeMax = 125 + (int) (numPlayers > 1 ? NPC.lifeMax * 0.15 * numPlayers : 0);     

        public override bool? DrawHealthBar (byte hbPosition, ref float scale, ref Vector2 position)
            => false;

        public override void AI () {
            NPC.direction = 0;
            float maxRotation = 0.2f;
            int max = 4000;
            float current = (float) Timer / max;
            Timer += Main.rand.NextFloat() * 3f * ((current + 0.5f) * 5f);
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
                    NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 2, (int) NPC.Center.X, (int) NPC.Center.Y, NPC.type);
                }
            }
        }

        public override void HitEffect (int hitDirection, double damage) {
            int max = 4000;
            Death(Timer >= max);
        }

        private void Death (bool spawnBunny = false) {
            if (NPC.life <= 0) {
                if (Main.netMode != NetmodeID.Server) {
                    int gore = ModContent.Find<ModGore>("Consolaria/EggShellBig").Type;
                    var entitySource = NPC.GetSource_Death();
                    for (int i = 0; i < 2; i++) {
                        Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-2, 2), 0), gore);
                    }
                }
                int type = ModContent.NPCType<Lepus>();
                if (spawnBunny && NPC.CountNPCS(type) < 5 && NPC.CountNPCS(type) > 0) {
                    if (Main.netMode != NetmodeID.Server) {
                        SoundStyle style = new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/EggCrack") { Volume = 0.8f};
                        SoundEngine.PlaySound(style, NPC.Center);
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient) {
                        return;
                    }
                    int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.Center.X, (int) NPC.Center.Y, type);
                    Main.npc [index].ai [1] = 2f;
                    Main.npc [index].Opacity = 1f;
                    Main.npc [index].TargetClosest();
                    Main.npc [index].netUpdate = true;
                    if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs) {
                        NetMessage.SendData(MessageID.SyncNPC, number: index);
                    }
                }
                NPC.netUpdate = true;
            }
        }
    }
}