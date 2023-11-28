﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
	public class DragonSkull : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        public override void SetStaticDefaults () {
			Main.npcFrameCount [NPC.type] = 3;

            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            BestiaryText = this.GetLocalization("Bestiary");
        }

		public override void SetDefaults () {
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

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.Banners.DragonSkullBanner>();
		}

		public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
            });
		}

		private int currentFrame;
		public override void FindFrame (int frameHeight) {
			NPC.frameCounter++;
			if (NPC.frameCounter > 6) {
				currentFrame++;
				NPC.frameCounter = 0;
			}
			if (currentFrame >= Main.npcFrameCount [NPC.type]) currentFrame = 0;
			NPC.frame.Y = currentFrame * frameHeight;
		}

		public override void AI ()
			=> NPC.velocity *= 1.015f;

		public override Color? GetAlpha (Color lightColor)
			=> new Color(255, 255, 255, 200);

		public override void OnHitPlayer (Player target, Player.HurtInfo hurtInfo) {
			if (Main.rand.NextBool(33))
				target.AddBuff(BuffID.Cursed, 240);
		}

		public override void HitEffect (NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server)
				return;

			if (NPC.life <= 0) {
				for (int k = 0; k < 20; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
				}
			}
			else {
				for (int k = 0; k < hit.Damage / NPC.lifeMax * 50.0; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, hit.HitDirection, -1f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, hit.HitDirection, -1f, 0, default, 0.7f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, hit.HitDirection, -1f, 0, default, 0.7f);
				}
			}
		}

		public override void ModifyNPCLoot (NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ItemID.Nazar, 100));
			npcLoot.Add(ItemDropRule.Common(ItemID.TallyCounter, 100));
			npcLoot.Add(ItemDropRule.Common(ItemID.GoldenKey, 65));
			npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 1, 3));
		}

		public override float SpawnChance (NPCSpawnInfo spawnInfo)
			=> SpawnCondition.Dungeon.Chance * 0.005f;
	}
}