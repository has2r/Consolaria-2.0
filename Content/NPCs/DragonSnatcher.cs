using Consolaria.Content.Items.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
	public class DragonSnatcher : ModNPC {

		private int timer = 0;
		private float posX = 0f;
		private float posY = 0f;
		private bool spawned = false;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault("Dragon Snatcher");
			Main.npcFrameCount [NPC.type] = 3;

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
				SpecificallyImmuneTo = new int [] {
					BuffID.Poisoned,
					BuffID.Confused
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				CustomTexturePath = "Consolaria/Assets/Textures/Bestiary/DragonSnatcher_Bestiary",
				Rotation = (float) Math.PI / 2f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults () {
			int width = 30; int height = width;
			NPC.Size = new Vector2(width, height);

			NPC.damage = 30;
			NPC.defense = 10;
			NPC.lifeMax = 80;

			NPC.value = Item.buyPrice(silver: 1, copper: 10);
			NPC.knockBackResist = 0f;

			NPC.noGravity = true;
			NPC.noTileCollide = true;

			NPC.aiStyle = -1;
			AnimationType = NPCID.Snatcher;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.DragonSnatcherBanner>();
		}

		public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
				new FlavorTextBestiaryInfoElement("This species of Snatcher learned to repurpose the stingers of Hornets that fell prey to it to hunt bigger targets.")
			});
		}

		public override void AI () {
			Player player = Main.player [NPC.target];
			NPC.TargetClosest(true);
			timer++;

			int i = (int) (NPC.Center.X) / 16;
			int j = (int) (NPC.Center.Y) / 16;
			while (j < Main.maxTilesY - 10 && Main.tile [i, j] != null && (!WorldGen.SolidTile2(i, j) && Main.tile [i - 1, j] != null) && (!WorldGen.SolidTile2(i - 1, j) && Main.tile [i + 1, j] != null && !WorldGen.SolidTile2(i + 1, j)))
				j += 2;

			int tile = j - 1;
			float worldY = tile * 16;
			if (!spawned) {
				spawned = true;
				NPC.position.Y = worldY;
				posX = player.position.X + (player.width * 0.5f);
				posY = player.position.Y + (player.height * 0.5f);
				NPC.ai [1] = NPC.position.X + NPC.width / 2;
				NPC.ai [2] = NPC.position.Y + NPC.height / 2;
				NPC.netUpdate = true;
			}

			if (timer > 180) {
				timer = 0;
				posX = player.position.X + (player.width * 0.5f);
				posY = player.position.Y + (player.height * 0.5f);
				NPC.netUpdate = true;
			}

			else if (timer > 110 || NPC.Distance(new Vector2(NPC.ai [1], NPC.ai [2])) > 450) {
				Vector2 vector_ = new Vector2(NPC.position.X + (NPC.width * 0.5f) - player.position.X + (player.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f) - player.position.Y + (player.height * 0.5f));
				posX = NPC.ai [1] - vector_.X * 1f;
				posY = NPC.ai [2] - vector_.Y * 1f;
			}

			if (timer == 100 && player.Distance(NPC.Center) < 250f) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(6f, -6f).RotatedBy(NPC.rotation + 180), ProjectileID.JungleSpike, NPC.damage / 2, 1, player.whoAmI);
				NPC.netUpdate = true;
			}

			if (posX < NPC.position.X) {
				if (NPC.velocity.X > -4) { NPC.velocity.X -= 0.25f; }
			}
			else if (posX > NPC.Center.X) {
				if (NPC.velocity.X < 4) { NPC.velocity.X += 0.25f; }
			}
			if (posY < NPC.position.Y) {
				if (NPC.velocity.Y > -4) NPC.velocity.Y -= 0.25f;
			}
			else if (posY > NPC.Center.Y) {
				if (NPC.velocity.Y < 4) NPC.velocity.Y += 0.25f;
			}

			NPC.rotation = ((float) Math.Atan2(player.Center.Y - (double) NPC.Center.Y, player.Center.X - (double) NPC.Center.X) + 3.14f) * 1f + ((float) Math.Atan2(NPC.velocity.Y, NPC.velocity.X)) * 0.1f;
		}

		public override bool PreDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = (Texture2D) ModContent.Request<Texture2D>("Consolaria/Assets/Textures/NPCs/DragonSnatcherChain");
			Vector2 vector = new(NPC.Center.X, NPC.Center.Y);
			float drawPosX = NPC.ai [1] - vector.X;
			float drawPosY = NPC.ai [2] - vector.Y;

			float rotation = (float) Math.Atan2(drawPosY, drawPosX) - 1.57f;
			bool flag = true;
			while (flag) {
				float drawPos = (float) Math.Sqrt(drawPosX * drawPosX + drawPosY * drawPosY);
				if (drawPos < 28f) flag = false;
				else {
					drawPos = 28f / drawPos;
					drawPosX *= drawPos;
					drawPosY *= drawPos;
					vector.X += drawPosX;
					vector.Y += drawPosY;
					drawPosX = NPC.ai [1] - vector.X;
					drawPosY = NPC.ai [2] - vector.Y;

					Main.spriteBatch.Draw(texture, new Vector2(vector.X - Main.screenPosition.X, vector.Y - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), drawColor, rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
				}
			}
			return true;
		}

		public override void HitEffect (int hitDirection, double damage) {
			if (Main.netMode == NetmodeID.Server)
				return;

			if (NPC.life <= 0) {
				for (int j = 0; j < 20; ++j)
					Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.JungleGrass, 0f, 2f);

				Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/DSnatcherGore1").Type);
				for (int i = 0; i < 2; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), ModContent.Find<ModGore>("Consolaria/DSnatcherGore2").Type);
			}
		}

		public override void ModifyNPCLoot (NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Cabbage>(), 20));
			npcLoot.Add(ItemDropRule.Common(ItemID.Stinger, 2));
		}

		public override float SpawnChance (NPCSpawnInfo spawnInfo)
			=> SpawnCondition.UndergroundJungle.Chance * 0.005f;
	}
}