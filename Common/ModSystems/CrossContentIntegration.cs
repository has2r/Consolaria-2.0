using Consolaria.Content.Items.BossDrops.Lepus;
using Consolaria.Content.Items.BossDrops.Ocram;
using Consolaria.Content.Items.BossDrops.Turkor;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.NPCs.Bosses.Lepus;
using Consolaria.Content.NPCs.Bosses.Ocram;
using Consolaria.Content.NPCs.Bosses.Turkor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Common {
    public class CrossContentIntegration : ModSystem {

        private int bossTypeLepus = ModContent.NPCType<Lepus>();
        private int bossTypeTurkor = ModContent.NPCType<TurkortheUngrateful>();
        private int bossTypeOcram = ModContent.NPCType<Ocram>();

        public override void PostSetupContent () {
<<<<<<< Updated upstream
            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist)) {

                if (bossChecklist.Version < new Version(1, 3, 1)) {
                    return;
                }
                bossChecklist.Call(
                    "AddBoss",
                    Mod,
                    "Lepus",
                    ModContent.NPCType<Lepus>(),
                    1.8f,
                    () => DownedBossSystem.downedLepus,
                    () => true,
                    new List<int> {
=======
            DoBossChecklistIntegration();
            DoFargosIntegration();
            DoAchievementModIntegration();
        }
        private void DoBossChecklistIntegration () {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod)) {
                return;
            }
            if (bossChecklistMod.Version < new Version(1, 6)) {
                return;
            }

            string lepusInternalName = "Lepus";
            string turkorInternalName = "Turkor";
            string ocramInternalName = "Ocram";

            float lepusWeight = 1.8f;
            float turkorWeight = 5.75f;
            float ocramWeight = 12f;

            Func<bool> downedLepus = () => DownedBossSystem.downedLepus;
            Func<bool> downedTurkor = () => DownedBossSystem.downedTurkor;
            Func<bool> downedOcram = () => DownedBossSystem.downedOcram;

            int lepusSpawnItem = ModContent.ItemType<SuspiciousLookingEgg>();
            int turkorSpawnItem = ModContent.ItemType<CursedStuffing>();
            int ocramSpawnItem = ModContent.ItemType<SuspiciousLookingSkull>();

            List<int> lepusCollectibles = new List<int>()
            {
>>>>>>> Stashed changes
                    ModContent.ItemType<LepusMask>(),
                    ModContent.ItemType<LepusTrophy>(),
                    ModContent.ItemType<LepusMusicBox>(),
                    ModContent.ItemType<LepusRelic>(),
                    ModContent.ItemType<RabbitFoot>()
                    },
                    ModContent.ItemType<SuspiciousLookingEgg>(),
                    $"Use a [i:" + ModContent.ItemType<SuspiciousLookingEgg>() + "] at day time",
                    "Lepus jumps away into sunset!"
                );
                bossChecklist.Call(
                   "AddBoss",
                   Mod,
                   "Turkor The Ungrateful",
                   ModContent.NPCType<TurkortheUngrateful>(),
                   5.75f,
                   () => DownedBossSystem.downedTurkor,
                   () => true,
                   new List<int> {
                    ModContent.ItemType<TurkorMask>(),
                    ModContent.ItemType<TurkorTrophy>(),
                    ModContent.ItemType<TurkorMusicBox>(),
                    ModContent.ItemType<TurkorRelic>(),
                    ModContent.ItemType<FruitfulPlate>()
                   },
                   ModContent.ItemType<CursedStuffing>(),
                   $"Use a [i:" + ModContent.ItemType<CursedStuffing>() + "] after summoning a pet turkey",
                   "Turkor the Ungrateful escapes from the dinner plate!",
                   (SpriteBatch spriteBatch, Rectangle rect, Color color) => {
                       Texture2D texture = ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Bestiary/Turkor_Bestiary").Value;
                       Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                       spriteBatch.Draw(texture, centered, color);
                   },
                   "Consolaria/Content/NPCs/Bosses/Turkor/TurkortheUngratefulHead_Head_Boss"
                );
                bossChecklist.Call(
                  "AddBoss",
                  Mod,
                  "Ocram",
                  ModContent.NPCType<Ocram>(),
                  12f,
                  () => DownedBossSystem.downedOcram,
                  () => true,
                  new List<int> {
                    ModContent.ItemType<OcramMask>(),
                    ModContent.ItemType<OcramTrophy>(),
                    ModContent.ItemType<OcramMusicBox>(),
                    ModContent.ItemType<OcramRelic>(),
                    ModContent.ItemType<CursedFang>()
                  },
                  ModContent.ItemType<SuspiciousLookingSkull>(),
                  $"Use a [i:" + ModContent.ItemType<SuspiciousLookingSkull>() + "] at night",
                  "Ocram disappears back into shadows!",
                  (SpriteBatch spriteBatch, Rectangle rect, Color color) => {
                      Texture2D texture = ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Bestiary/Ocram_Bestiary").Value;
                      Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                      spriteBatch.Draw(texture, centered, color);
                  }
                );
            }

            if (ModLoader.TryGetMod("Fargowiltas", out Mod fargos)) {
                fargos.Call("AddSummon", 1.8f, "Consolaria", "SuspiciousLookingEgg", () => DownedBossSystem.downedLepus, 60000);
                fargos.Call("AddSummon", 5.75f, "Consolaria", "CursedStuffing", () => DownedBossSystem.downedLepus, 180000);
                fargos.Call("AddSummon", 12f, "Consolaria", "SuspiciousLookingSkull", () => DownedBossSystem.downedLepus, 500000);
            }
        }

        private void DoAchievementModIntegration () {
            if (ModLoader.TryGetMod("TMLAchievements", out Mod achievement))
                achievement.Call("AddAchievement", this, "LepusAchievement", AchievementCategory.Slayer, "Consolaria/Assets/Achievements/LepusAchievement", null, false, true, 5f, new string [] { "Kill_" + bossTypeLepus });
        }
    }
}