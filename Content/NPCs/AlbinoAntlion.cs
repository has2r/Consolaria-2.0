using Consolaria.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs
{	
	public class AlbinoAntlion : ModNPC
	{
		public override void SetStaticDefaults()  {
			DisplayName.SetDefault("Albino Antlion");
			Main.npcFrameCount[NPC.type] = 5;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int[] {
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
			int width = 24; int height = width;
			NPC.Size = new Vector2(width, height);

			NPC.damage = 10;
			NPC.defense = 10;
			NPC.lifeMax = 45;

			NPC.value = Item.buyPrice(copper: 65);
			NPC.knockBackResist = 0f;
			NPC.behindTiles = true;

			NPC.aiStyle = 19;
			AnimationType = NPCID.Antlion;

			NPC.HitSound = SoundID.NPCHit31;
			NPC.DeathSound = SoundID.NPCDeath34;

		    //Banner = NPC.type;
		    //BannerItem = mod.ItemType("ArchDemonBanner");
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
			NPC.lifeMax *= 1;
			NPC.damage *= 1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement("Antlion, but more powerful, I guess...")
			});
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D bodyTexture = (Texture2D)ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/AlbinoAntlionBody");
			Vector2 position = new Vector2(NPC.Center.X, NPC.Center.Y) - Main.screenPosition;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
			var spriteEffects = NPC.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(texture, new Vector2(position.X, position.Y), NPC.frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, spriteEffects, 0f);
			spriteBatch.Draw(bodyTexture, new Vector2(position.X, position.Y + 24), NPC.frame, drawColor, 0, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			return false;
        }

		public override void HitEffect(int hitDirection, double damage) {
			Dust.NewDust(NPC.position, NPC.width, NPC.height, 185, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/AlbinoAntlionGore").Type);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 59, 2.5f * hitDirection, -2.5f, 0, default(Color), 1f);		
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
			=> npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AlbinoMandible>(), 30));
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			=> SpawnCondition.OverworldDayDesert.Chance * 0.05f;
	}
}
