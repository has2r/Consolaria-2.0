using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Lepus {
    public class ChocolateEgg : ModNPC {
        public override void SetStaticDefaults () {
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Venom] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults () {
            int width = 20; int height = 26;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = -1;
            NPC.friendly = true;

            NPC.damage = 0;
            NPC.defense = 3;

            NPC.lifeMax = 30;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.noTileCollide = false;
        }

        public override void AI () {
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                NPC.homeless = false;
                NPC.homeTileX = -1;
                NPC.homeTileY = -1;
                NPC.netUpdate = true;
            }
            NPC.spriteDirection = 0;
            NPC.velocity.X = 0f;
            NPC.velocity.Y = 5f;
        }

        public override void UpdateLifeRegen (ref int damage) {
            if (NPC.lifeRegen > 0)
                NPC.lifeRegen = 0;

            NPC.lifeRegen -= 5;
            if (damage < 1) damage = 1;
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (NPC.life <= 0) {
                int item = Item.NewItem(NPC.GetSource_Death(), NPC.Center, 0, 0, ItemID.Star, Main.rand.Next(1, 3), false, 0, false, false);
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
                item = Item.NewItem(NPC.GetSource_Death(), NPC.Center, 0, 0, ItemID.Heart, Main.rand.Next(1, 3), false, 0, false, false);
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f, 0f, 0f, 0, 0, 0);
                if (Main.netMode != NetmodeID.Server) {
                    int chocolateEggGore = ModContent.Find<ModGore>("Consolaria/ChocolateEggGore").Type;
                    for (int i = 0; i < 1; i++) {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), chocolateEggGore);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), chocolateEggGore);
                    }
                }
                SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/EggCrack") { Volume = 0.7f, Pitch = -0.15f }, NPC.Center);
            }
        }
    }
}