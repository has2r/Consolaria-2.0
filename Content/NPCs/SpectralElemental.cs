using Consolaria.Content.Projectiles.Enemies;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Consolaria.Content.NPCs {
    public class SpectralElemental : ModNPC {
        public static LocalizedText BestiaryText {
            get; private set;
        }

        public override void SetStaticDefaults () {
            Main.npcFrameCount [NPC.type] = 15;

            NPCID.Sets.SpecificDebuffImmunity [Type] [BuffID.Poisoned] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            BestiaryText = this.GetLocalization("Bestiary");
        }

        public override void SetDefaults () {
            int width = 40; int height = 24;
            NPC.Size = new Vector2(width, height);

            NPC.lifeMax = 400;
            NPC.damage = 40;

            NPC.defense = 30;
            NPC.knockBackResist = 0.6f;
            NPC.rarity = 1;

            NPC.value = Item.buyPrice(silver: 10);

            NPC.aiStyle = 3;
            AIType = NPCID.ChaosElemental;
            AnimationType = NPCID.ChaosElemental;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Placeable.Banners.SpectralElementalBanner>();
        }

        public override void SetBestiary (BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement [] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow,
                new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
            });
        }

        public override void AI () {
            Player player = Main.player [NPC.target];
            NPC.localAI [0]++;
            Vector2 playerPos = new(player.position.X, player.position.Y);
            if (NPC.localAI [0] >= 300 + Main.rand.Next(0, 180) && Vector2.Distance(NPC.Center, player.Center) < 300f) {
                for (int I = 0; I < 20; I++)
                    Dust.NewDust(NPC.position, NPC.width - (Main.rand.Next(NPC.width)), NPC.height - (Main.rand.Next(NPC.height)), DustID.BlueFairy, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 100, default, 1.1f);
                Teleport(playerPos);
                NPC.localAI [0] = 0;
            }
            Lighting.AddLight(NPC.Center, new Vector3(0f, 0.7f, 0.9f));
        }

        private void Teleport (Vector2 playerPosition) {
            Player player = Main.player [NPC.target];
            Vector2 teleportTo = new(playerPosition.X + 140 * player.direction, playerPosition.Y);
            Vector2 teleportFrom = new(NPC.position.X, NPC.position.Y);
            Vector2 NormalizedVec = new(0, -0.1f);
            NormalizedVec.Normalize();
            if (!Collision.SolidCollision(teleportTo, 16, 16)) {
                SoundEngine.PlaySound(SoundID.Item4, teleportFrom);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), teleportFrom, NormalizedVec, ModContent.ProjectileType<SpectralBomb>(), 30, 4, player.whoAmI);
                NPC.position = teleportTo;
            }
        }

        public override void HitEffect (NPC.HitInfo hit) {
            if (Main.netMode == NetmodeID.Server)
                return;

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FrostHydra, 2.5f * hit.HitDirection, -2.5f, 0, default, 0.8f);
            if (NPC.life <= 0) {
                for (int i = 0; i < 25; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FrostHydra, 2.5f * hit.HitDirection, -2.5f, 0, default, 1f);
            }
        }

        public override bool PreDraw (SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D texture = (Texture2D) ModContent.Request<Texture2D>(Texture);
            SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount [NPC.type] / 2);
            spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale), new Rectangle?(NPC.frame), Color.White * 0.7f, NPC.rotation, origin, NPC.scale, effects, 0f);
            for (int i = 1; i < NPC.oldPos.Length; i++) {
                Color color = Lighting.GetColor((int) (NPC.position.X + NPC.width * 0.5) / 16, (int) ((NPC.position.Y + NPC.height * 0.5) / 16.0));
                Color color2 = color;
                color2 = Color.Lerp(color2, Color.LightSkyBlue, 0.8f);
                color2 = NPC.GetAlpha(color2);
                color2 *= (NPC.oldPos.Length - i) / 15f;
                spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale / Main.npcFrameCount [NPC.type] + 4f + origin.Y * NPC.scale) - NPC.velocity * i * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, origin, NPC.scale, effects, 0f);
            }
            return false;
        }

        public override void ModifyNPCLoot (NPCLoot npcLoot) {
            var elementalDropRules = Main.ItemDropsDB.GetRulesForNPCID(NPCID.ChaosElemental, false);
            foreach (var elementalDropRule in elementalDropRules)
                npcLoot.Add(elementalDropRule);
        }

        public override float SpawnChance (NPCSpawnInfo spawnInfo)
            => (spawnInfo.Player.ZoneHallow && spawnInfo.SpawnTileY > Main.rockLayer && !spawnInfo.Player.ZoneUnderworldHeight && (spawnInfo.SpawnTileType == TileID.Pearlsand || spawnInfo.SpawnTileType == TileID.Pearlstone || spawnInfo.SpawnTileType == TileID.HallowedGrass || spawnInfo.SpawnTileType == TileID.HallowedIce)) ?
            SpawnCondition.Cavern.Chance * 0.025f : 0f;
    }
}