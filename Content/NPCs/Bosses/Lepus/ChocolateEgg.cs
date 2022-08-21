using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Lepus {
    public class ChocolateEgg : ModNPC {
        public override void SetStaticDefaults () {
            DisplayName.SetDefault("Chocolate Egg");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults () {
            int width = 20; int height = 26;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = 0;

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

        public override void HitEffect (int hitDirection, double damage) {
            if (NPC.life <= 0) {
                if (Main.netMode != NetmodeID.Server) {
                    int chocolateEggGore = ModContent.Find<ModGore>("Consolaria/ChocolateEggGore").Type;
                    for (int i = 0; i < 1; i++) {
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), chocolateEggGore);
                        Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), chocolateEggGore);
                    }
                }
                SoundEngine.PlaySound(new SoundStyle($"{nameof(Consolaria)}/Assets/Sounds/EggCrack") { Volume = 0.7f, Pitch = 0.15f }, NPC.Center);
            }
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            npcLoot.Add(ItemDropRule.Common(ItemID.Star, 1, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.Heart, 1, 1, 3));
        }
    }
}