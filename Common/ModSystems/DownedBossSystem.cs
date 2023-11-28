using Consolaria.Common.ModSystems;
using Consolaria.Content.Items.Consumables;
using Consolaria.Content.NPCs.Bosses.Lepus;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Common.ModSystems {
	public class DownedBossSystem : ModSystem {
		public static bool downedLepus = false;
		public static bool downedTurkor = false;
		public static bool downedOcram = false;

		public override void OnWorldLoad () {
			downedLepus = false;
			downedTurkor = false;
			downedOcram = false;
		}

		public override void OnWorldUnload () {
			downedLepus = false;
			downedTurkor = false;
			downedOcram = false;
		}

		public override void SaveWorldData (TagCompound tag) {
			if (downedLepus) tag ["downedLepus"] = true;
			if (downedTurkor) tag ["downedTurkor"] = true;
			if (downedOcram) tag ["downedOcram"] = true;
		}

		public override void LoadWorldData (TagCompound tag) {
			downedLepus = tag.ContainsKey("downedLepus");
			downedTurkor = tag.ContainsKey("downedTurkor");
			downedOcram = tag.ContainsKey("downedOcram");
		}

		public override void NetSend (BinaryWriter writer) {
			var flags = new BitsByte();
			flags [0] = downedLepus;
			flags [1] = downedTurkor;
			flags [2] = downedOcram;
			writer.Write(flags);
		}

		public override void NetReceive (BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			downedLepus = flags [0];
			downedTurkor = flags [1];
			downedOcram = flags [2];
		}

		public override void PostUpdateWorld () {

		}
	}

	public class RabbitInvasionBiome : ModBiome {
		private const string MUSIC_PATH = "Consolaria/Assets/Music/Lepus";
		public override int Music => MusicLoader.GetMusicSlot(MUSIC_PATH);

		public override bool IsBiomeActive (Player player) {
			Vector2 position = player.position;
			int playerX = (int) position.X / 16;
			int playerY = (int) position.Y / 16;
			int middle = Main.maxTilesX / 2;
			return playerY < Main.worldSurface && playerX > middle - 750 && playerX < middle + 750 && !NPC.AnyNPCs(ModContent.NPCType<Lepus>()) && RabbitInvasion.rabbitInvasion;
		}
	}

	public class RabbitInvasionPlayer : ModPlayer {
		public override void OnEnterWorld () {
			if (RabbitInvasion.rabbitInvasion) {
				string text = "Bunnies are everywhere!";
				Main.NewText(text, new Color(50, 255, 130));
			}
		}
	}

	public class RabbitInvasion : ModSystem {
		public static bool rabbitInvasion = false;
		public static int rabbitKilledCount = 0;

		public override void OnWorldLoad () {
			rabbitInvasion = false;
		}

		public override void OnWorldUnload () {
			rabbitInvasion = false;
		}

		public override void SaveWorldData (TagCompound tag) {
			if (rabbitInvasion) {
				tag ["rabbitInvasion"] = true;
			}
		}

		public override void LoadWorldData (TagCompound tag) {
			rabbitInvasion = tag.ContainsKey("rabbitInvasion");
		}

		public override void NetSend (BinaryWriter writer) {
			var flags = new BitsByte();
			flags [0] = rabbitInvasion;
			writer.Write(flags);
		}

		public override void NetReceive (BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			rabbitInvasion = flags [0];
		}
	}

	public class RabbitInvasionNPCs : GlobalNPC {
		public override void ModifyNPCLoot (NPC npc, NPCLoot npcLoot) {
			if (npc.type == NPCID.CrimsonBunny || npc.type == NPCID.CorruptBunny) {
				RabbitInvasionDropCondition lepusDropCondition2 = new();
				IItemDropRule conditionalRule2 = new LeadingConditionRule(lepusDropCondition2);
				conditionalRule2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<GoldenCarrot>(), 4));
				npcLoot.Add(conditionalRule2);
			}
		}

		public override void EditSpawnRate (Player player, ref int spawnRate, ref int maxSpawns) {
			Vector2 position = player.position;
			int playerX = (int) position.X / 16;
			int playerY = (int) position.Y / 16;
			int middle = Main.maxTilesX / 2;
			bool condition = playerY < Main.worldSurface && playerX > middle - 750 && playerX < middle + 750 && !NPC.AnyNPCs(ModContent.NPCType<Lepus>());
			if (RabbitInvasion.rabbitInvasion && condition) {
				spawnRate = (int) (spawnRate * 0.1f);
				maxSpawns = (int) (maxSpawns * 4f);
			}
		}

		public override void OnKill (NPC npc) {
			if (!RabbitInvasion.rabbitInvasion || RabbitInvasion.rabbitKilledCount == -1) {
				return;
			}
			if (npc.type == ModContent.NPCType<DisasterBunny>() || npc.type == (WorldGen.crimson ? NPCID.CrimsonBunny : NPCID.CorruptBunny)) {
				if (RabbitInvasion.rabbitKilledCount < 100) {
					RabbitInvasion.rabbitKilledCount++;
				} else {
					RabbitInvasion.rabbitKilledCount = -1;
					Player player = Main.player [npc.target];
					SoundEngine.PlaySound(SoundID.Roar);
					int type = ModContent.NPCType<Lepus>();
					if (Main.netMode != NetmodeID.MultiplayerClient) {
						NPC.SpawnOnPlayer(player.whoAmI, type);
					} else if (Main.netMode != NetmodeID.SinglePlayer) {
						NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
					}
				}
			}
		}

		public override void EditSpawnPool (IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
			Vector2 position = spawnInfo.Player.position;
			int playerX = (int) position.X / 16;
			int playerY = (int) position.Y / 16;
			int middle = Main.maxTilesX / 2;
			bool condition = playerY < Main.worldSurface && playerX > middle - 750 && playerX < middle + 750 && !NPC.AnyNPCs(ModContent.NPCType<Lepus>());
			if (RabbitInvasion.rabbitInvasion && condition) {
				pool.Clear();
				pool.Add(ModContent.NPCType<DisasterBunny>(), 100f);
				pool.Add(WorldGen.crimson ? NPCID.CrimsonBunny : NPCID.CorruptBunny, 25f);
			}
		}
	}
}