using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
	public class AlbinoAntlion : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        public override void SetStaticDefaults () {
			Main.npcFrameCount [NPC.type] = 5;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int [] {
					BuffID.Confused
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				CustomTexturePath = "Consolaria/Assets/Textures/Bestiary/AlbinoAntlion_Bestiary",
				Position = new Vector2(0f, 4f)
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            BestiaryText = this.GetLocalization("Bestiary");
        }

		public override void SetDefaults () {
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

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.AlbinoAntlionBanner>();
		}

		public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
			});
		}

		public override bool PreDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
			Texture2D bodyTexture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/AlbinoAntlionBody");
			Vector2 position = new Vector2(NPC.position.X, NPC.position.Y) - Main.screenPosition;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
			var spriteEffects = NPC.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(bodyTexture, new Vector2(position.X, position.Y + 20), NPC.frame, drawColor, 0, drawOrigin, NPC.scale, SpriteEffects.None, 1f);
			spriteBatch.Draw(texture, new Vector2(position.X, position.Y), NPC.frame, drawColor, NPC.rotation / 2, drawOrigin, NPC.scale, spriteEffects, 0f);
			return false;
		}

		public override void HitEffect (NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server)
				return;

			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand, 2.5f * hit.HitDirection, -2.5f, 0, Color.White, 0.7f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/AlbinoAntlionGore").Type);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sand, 2.5f * hit.HitDirection, -2.5f, 0, Color.White, 1f);
			}
		}

		public override void ModifyNPCLoot (NPCLoot npcLoot) {
            var antlionsDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Antlion, false);
            foreach (var antlionsDropRule in antlionsDropRules)
                npcLoot.Add(antlionsDropRule);
        }

		public override float SpawnChance (NPCSpawnInfo spawnInfo)
			=> SpawnCondition.OverworldDayDesert.Chance * 0.025f;
	}
}