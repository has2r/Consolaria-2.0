using Consolaria.Common.ModSystems;
using Consolaria.Content.Items.Mounts;
using Consolaria.Content.Items.Pets;
using Consolaria.Content.Items.Placeable;
using Consolaria.Content.Items.Vanity;
using Consolaria.Content.NPCs.Bosses.Lepus;
using Consolaria.Content.NPCs.Bosses.Ocram;
using Consolaria.Content.NPCs.Bosses.Turkor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace Consolaria.Common {
    public class CrossContentIntegration : ModSystem {
        public override void PostSetupContent () {
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
            float ocramWeight = 13f;

            Func<bool> downedLepus = () => DownedBossSystem.downedLepus;
            Func<bool> downedTurkor = () => DownedBossSystem.downedTurkor;
            Func<bool> downedOcram = () => DownedBossSystem.downedOcram;

            int lepusSpawnItem = ModContent.ItemType<Content.Items.Summons.SuspiciousLookingEgg>();
            List<int> turkorSpawnItems = new List<int>() { ModContent.ItemType<Content.Items.Summons.CursedStuffing>(), ModContent.ItemType<TurkeyFeather>() };
            int turkorSpawnItem = ModContent.ItemType<Content.Items.Summons.CursedStuffing>();
            int ocramSpawnItem = ModContent.ItemType<Content.Items.Summons.SuspiciousLookingSkull>();

            List<int> lepusCollectibles = new List<int>()
            {
                    ModContent.ItemType<LepusMask>(),
                    ModContent.ItemType<LepusTrophy>(),
                    ModContent.ItemType<LepusMusicBox>(),
                    ModContent.ItemType<LepusRelic>(),
                    ModContent.ItemType<RabbitFoot>()
            };
            List<int> turkorCollectibles = new List<int>()
            {
                    ModContent.ItemType<TurkorMask>(),
                    ModContent.ItemType<TurkorTrophy>(),
                    ModContent.ItemType<TurkorMusicBox>(),
                    ModContent.ItemType<TurkorRelic>(),
                    ModContent.ItemType<FruitfulPlate>()
            };
            List<int> ocramCollectibles = new List<int>()
            {
                    ModContent.ItemType<OcramMask>(),
                    ModContent.ItemType<OcramTrophy>(),
                    ModContent.ItemType<OcramMusicBox>(),
                    ModContent.ItemType<EerieOcramMusicBox>(),
                    ModContent.ItemType<OcramRelic>(),
                    ModContent.ItemType<CursedFang>()
            };

            var turkorCustomPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Bestiary/Turkor_Bestiary").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            var ocramCustomPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Bestiary/Ocram_Bestiary").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                lepusInternalName,
                lepusWeight,
                downedLepus,
                ModContent.NPCType<Lepus>(),
                new Dictionary<string, object>() {
                    ["spawnItems"] = lepusSpawnItem,
                    ["collectibles"] = lepusCollectibles
                }
            );
            bossChecklistMod.Call(
               "LogBoss",
               Mod,
               turkorInternalName,
               turkorWeight,
               downedTurkor,
               ModContent.NPCType<TurkortheUngrateful>(),
               new Dictionary<string, object>() {
                   ["spawnItems"] = turkorSpawnItems,
                   ["collectibles"] = turkorCollectibles,
                   ["customPortrait"] = turkorCustomPortrait
               }
           );
            bossChecklistMod.Call(
               "LogBoss",
               Mod,
               ocramInternalName,
               ocramWeight,
               downedOcram,
               ModContent.NPCType<Ocram>(),
               new Dictionary<string, object>() {
                   ["spawnItems"] = ocramSpawnItem,
                   ["collectibles"] = ocramCollectibles,
                   ["customPortrait"] = ocramCustomPortrait
               }
           );
        }

        private void DoFargosIntegration () {
            if (ModLoader.TryGetMod("Fargowiltas", out Mod fargos)) {
                fargos.Call("AddSummon", 1.8f, "Consolaria", "SuspiciousLookingEgg", () => DownedBossSystem.downedLepus, 60000);
                fargos.Call("AddSummon", 5.75f, "Consolaria", "CursedStuffing", () => DownedBossSystem.downedTurkor, 180000);
                fargos.Call("AddSummon", 12f, "Consolaria", "SuspiciousLookingSkull", () => DownedBossSystem.downedOcram, 500000);
            }
        }

        private void DoAchievementModIntegration () {
            if (ModLoader.TryGetMod("TMLAchievements", out Mod achievement))
                achievement.Call("AddAchievement", this, "LepusAchievement", AchievementCategory.Slayer, "Consolaria/Assets/Achievements/LepusAchievement", null, false, true, 5f, new string [] { "Kill_" + ModContent.NPCType<Lepus>() });
        }
    }
}