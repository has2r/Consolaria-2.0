using Consolaria.Content.Items.Summons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs.Lepus
{
    public class DisasterBunny : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Diseaster Bunny");
            Main.npcFrameCount[NPC.type] = 7;
        }

        public override void SetDefaults() {
            int width = 35; int height = 28;
            NPC.Size = new Vector2(width, height);

            NPC.aiStyle = 3;
            AIType = 47;
            AnimationType = 47;

            NPC.damage = 20;
            NPC.defense = 8;

            NPC.lifeMax = 72;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.noTileCollide = false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            NPC.lifeMax = 80;
            NPC.damage = 23;
            NPC.defense = 9;
        }

        public override void HitEffect(int hitDirection, double damage) {
            if (NPC.life <= 0)  {
                Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-4, 5)), ModContent.Find<ModGore>("Consolaria/DBG1").Type);
                Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-4, 5)), ModContent.Find<ModGore>("Consolaria/DBG2").Type);
                Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-4, 5)), ModContent.Find<ModGore>("Consolaria/DBG3").Type);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
            if (!NPC.AnyNPCs(ModContent.NPCType<Lepus>()))
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SuspiciousLookingEgg>(), 25)); //Drop Suspicious Looking Egg with a 1 out of 25 chance.
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return SpawnCondition.OverworldDaySlime.Chance * 0.2f;
        }
    }
}