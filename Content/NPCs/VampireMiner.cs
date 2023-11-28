using Consolaria.Content.Items.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
	public class VampireMiner : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        public override void SetStaticDefaults () {
			Main.npcFrameCount [NPC.type] = 15;

            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Bleeding] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
                Velocity = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            NPCID.Sets.ShimmerTransformToNPC [NPC.type] = NPCID.UndeadMiner;

            BestiaryText = this.GetLocalization("Bestiary");
        }

		public override void SetDefaults () {
			int width = 40; int height = width;
			NPC.Size = new Vector2(width, height);

			NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath2;

			NPC.damage = 25;
			NPC.defense = 3;

			NPC.lifeMax = 90;
			NPC.knockBackResist = 0.3f;

			NPC.value = Item.buyPrice(silver: 5);
			NPC.npcSlots = 1.5f;

			NPC.aiStyle = 3;
			AIType = 21;
			AnimationType = 21;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Placeable.Banners.VampireMinerBanner>();
		}

		public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
			});
		}

		public override void AI ()
			=> Lighting.AddLight(NPC.Center, new Vector3(0.8f, 0f, 0.7f));

		public override void OnHitPlayer (Player target, Player.HurtInfo hurtInfo) {
			if (Main.rand.NextBool(5))
				target.AddBuff(BuffID.Bleeding, 60 * 5);

			if (Main.rand.NextBool(2) && NPC.life < NPC.lifeMax) {
				for (int i = 0; i < 10; i++)
					Dust.NewDust(NPC.position, i, i, DustID.Blood, 2, 2, 100, default, 0.9f);

				int healLife = hurtInfo.Damage / 5;
				if (healLife > 0) {
					NPC.life += healLife;
					NPC.HealEffect(healLife, true);
				}
			}
		}

		public override void HitEffect (NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server)
				return;

			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.7f);
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/vampgore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/vampgore2").Type, 1f);
				for (int i = 0; i < 20; i++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f, 0, default, 1f);
			}
		}

		public override void ModifyNPCLoot (NPCLoot npcLoot) {
			var minerDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.UndeadMiner, false);
			foreach (var minerDropRule in minerDropRules)
				npcLoot.Add(minerDropRule);
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VialOfBlood>(), 25));
		}

		public override float SpawnChance (NPCSpawnInfo spawnInfo)
			=> SpawnCondition.Cavern.Chance * 0.008f;
	}
}