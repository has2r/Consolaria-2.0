﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
	public class ShadowHammer : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        public override void SetStaticDefaults () {
			Main.npcFrameCount [NPC.type] = 6;

            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.CursedInferno] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 0.8f,
				Position = new Vector2(20f, 10f),
				PortraitPositionXOverride = -5f,
				PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            BestiaryText = this.GetLocalization("Bestiary");
        }

		public override void SetDefaults () {
			int width = 40; int height = width;
			NPC.Size = new Vector2(width, height);

			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;

			NPC.damage = 80;
			NPC.defense = 20;

			NPC.lifeMax = 250;
			NPC.knockBackResist = 0.6f;
			NPC.rarity = 1;

			NPC.value = Item.buyPrice(silver: 15);

			NPC.noGravity = true;
			NPC.lavaImmune = true;

			NPC.aiStyle = 23;
			AIType = 83;
			AnimationType = 83;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.Banners.ShadowHammerBanner>();
		}

		public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption,
				new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
			});
		}

		public override void OnHitPlayer (Player target, Player.HurtInfo hurtInfo) {
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.CursedInferno, 60 * 5);
		}

		public override Color? GetAlpha (Color drawColor)
			=> Color.White * 0.8f;

		public override void HitEffect (NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server)
				return;

			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, 2.5f * hit.HitDirection, -2.5f, 0, default, 1f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity / 2f, 99, 1f);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, 2.5f * hit.HitDirection, -2.5f, 0, default, 1f);
			}
		}

		public override void ModifyNPCLoot (NPCLoot npcLoot) {
			var hammerDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.CursedHammer, false);
			foreach (var hammerDropRule in hammerDropRules)
				npcLoot.Add(hammerDropRule);
		}

		public override float SpawnChance (NPCSpawnInfo spawnInfo)
			=> (spawnInfo.Player.ZoneCorrupt && Main.hardMode && spawnInfo.SpawnTileY > Main.rockLayer) ?
			SpawnCondition.Corruption.Chance * 0.005f : 0f;
	}
}