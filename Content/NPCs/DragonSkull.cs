using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs
{	
	public class DragonSkull : ModNPC
	{
		public override void SetStaticDefaults()  {
			DisplayName.SetDefault("Dragon Skull");
			Main.npcFrameCount[NPC.type] = 3;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
					BuffID.Confused
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Velocity = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			int width = 56; int height = 28;
			NPC.Size = new Vector2(width, height);

			NPC.damage = 35;
			NPC.defense = 8;
			NPC.lifeMax = 75;

			NPC.value = Item.buyPrice(silver: 1, copper: 90);
			NPC.knockBackResist = 0.8f;

			NPC.noTileCollide = true;
			NPC.noGravity = true;

			NPC.aiStyle = NPCAIStyleID.CursedSkull;

			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath2;

		    //Banner = NPC.type;
		    //BannerItem = mod.ItemType("ArchDemonBanner");
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
			NPC.lifeMax *= 1;
			NPC.damage *= 1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
				new FlavorTextBestiaryInfoElement("Demon, but more powerful, I guess...")
			});
		}

		private int currentFrame;
		public override void FindFrame(int frameHeight) {
			NPC.frameCounter++;
			if (NPC.frameCounter > 6) {
				currentFrame++;
				NPC.frameCounter = 0;
			}
			if (currentFrame >= Main.npcFrameCount[NPC.type]) currentFrame = 0;
			NPC.frame.Y = currentFrame * frameHeight;
		}

		public override void AI()
			=> NPC.velocity *= 1.015f;

		public override Color? GetAlpha(Color lightColor)
			=> new Color(255, 255, 255, 200);
		

		public override void OnHitPlayer(Player target, int damage, bool crit) {
			if (Main.rand.Next(33) == 0) 
				target.AddBuff(BuffID.Cursed, 240);
		}

        public override void HitEffect(int hitDirection, double damage) {
			if (NPC.life <= 0) {
				for (int k = 0; k < 20; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				}
			}
			else {
				for (int k = 0; k < damage / NPC.lifeMax * 50.0; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hitDirection, -1f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hitDirection, -1f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hitDirection, -1f, 0, default, 0.7f);
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) { 
			npcLoot.Add(ItemDropRule.Common(ItemID.Nazar, 100));
			npcLoot.Add(ItemDropRule.Common(ItemID.TallyCounter, 100));
			npcLoot.Add(ItemDropRule.Common(ItemID.GoldenKey, 65));
			npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 1, 3));
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			=> SpawnCondition.Dungeon.Chance * 0.33f;
	}
}
