using Consolaria.Content.Items.BossDrops.Lepus;
using Consolaria.Content.Items.BossDrops.Ocram;
using Consolaria.Content.Items.BossDrops.Turkor;
using Consolaria.Content.Items.Summons;
using Consolaria.Content.NPCs.Bosses.Lepus;
using Consolaria.Content.NPCs.Ocram;
using Consolaria.Content.NPCs.Turkor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Consolaria.Common {
    public class BossChecklistIntegration : ModSystem {
        public override void PostSetupContent () {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist)) {
                return;
            }
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
               $"Use a [i:" + ModContent.ItemType<CursedStuffing>() + "] after summoning pet turkey",
               "Turkor the Ungrateful escapes from the dinner plate!",
               (SpriteBatch sb, Rectangle rect, Color color) => {
                   Texture2D texture = ModContent.Request<Texture2D>("Consolaria/Assets/Textures/Bestiary/Turkor_Bestiary").Value;
                   Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                   sb.Draw(texture, centered, color);
               },
               "Consolaria/Content/NPCs/Turkor/TurkortheUngratefulHead_Head_Boss"
            );
            bossChecklist.Call(
              "AddBoss",
              Mod,
              "Ocram",
              ModContent.NPCType<Ocram>(),
              11.9f,
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
              $"Use a [i:" + ModContent.ItemType<SuspiciousLookingSkull>() + "] at night after all mechanical bosses has been defeated",
              "Ocram disappears back into shadows!"
            );
        }
    }
}