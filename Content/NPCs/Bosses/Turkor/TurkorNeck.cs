﻿using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.Turkor {
    public class TurkorNeck : ModNPC {
		private ref float neck => ref NPC.ai [3];

		public override void SetStaticDefaults () {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

			NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Confused] = true;
		}

		public override void SetDefaults () {
			NPC.lifeMax = 1;
			NPC.knockBackResist = 0.5f;
			NPC.width = 18;
			NPC.height = 18;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.lavaImmune = true;
			NPC.alpha = 255;
			NPC.damage = 0;
		}

		public static Vector2 CenterPoint (Vector2 A, Vector2 B)
			=> new((A.X + B.X) / 2f, (A.Y + B.Y) / 2f);

		public static Vector2 CenterPoint1 (Vector2 A, Vector2 B)
			=> new((A.X + B.X - 50) / 2f, (A.Y + B.Y + 20) / 2f);

		public override void AI () {
			NPC.TargetClosest(true);
			if (neck == -1f && NPC.ai [2] > 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				NPC.realLife = NPC.whoAmI;
				Console.WriteLine(NPC.ai [2]);
				neck = (float) NPC.NewNPC(NPC.GetSource_FromAI(), (int) NPC.Center.X, (int) NPC.Center.Y + 20, ModContent.NPCType<TurkorNeck>(), NPC.whoAmI, 0, NPC.whoAmI);
				Main.npc [(int) neck].ai [2] = NPC.ai [2] - 1;
				Main.npc [(int) neck].ai [0] = NPC.whoAmI;
				Main.npc [(int) neck].ai [1] = NPC.ai [1]; // why net this? (not used afaik)
				Main.npc [(int) neck].ai [3] = -1f;
				Main.npc [(int) neck].realLife = NPC.whoAmI;
				Main.npc [(int) neck].position = Main.npc [(int) NPC.ai [1]].position;
				if (Main.netMode != NetmodeID.Server && neck < Main.maxNPCs) {
					NetMessage.SendData(MessageID.SyncNPC, number: (int) neck);
				}
				NPC.netUpdate = true;
			}
			NPC.alpha = Main.npc [(int) NPC.ai [0]].alpha;
			if (!Main.npc [(int) NPC.ai [0]].active && Main.netMode != NetmodeID.MultiplayerClient) {
				NPC.life = 0;
				NPC.HitEffect(0, 10.0);
				NPC.active = false;
				if (Main.netMode != NetmodeID.Server) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)), ModContent.Find<ModGore>("Consolaria/TurkorNeck").Type);
				}
			}

			if (neck != -1f && NPC.ai [2] > 0)
				NPC.Center = CenterPoint(Main.npc [(int) neck].Center, Main.npc [(int) NPC.ai [0]].Center);

			else if (NPC.ai [2] <= 0) {
				for (int k = 0; k < Main.maxNPCs; k++) {
					if (Main.npc [k].type == ModContent.NPCType<TurkortheUngrateful>() && Main.npc [k].active)
						NPC.Center = CenterPoint1(Main.npc [k].Center, Main.npc [(int) NPC.ai [0]].Center);
				}
			}
			NPC.direction = Main.player [NPC.target].Center.X < NPC.Center.X ? -1 : 1;
			NPC.rotation = (float) Math.Atan2(Main.npc [(int) NPC.ai [0]].Center.Y - NPC.Center.Y, Main.npc [(int) NPC.ai [0]].Center.X - NPC.Center.X) + 1.57f;
		}
	}
}