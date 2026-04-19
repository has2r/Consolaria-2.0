using Consolaria.Content.Items.Pets;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs;

public sealed class AlbinoSwarmer : ModNPC {
    public static LocalizedText BestiaryText { get; private set; }

    public override void Load() {
        On_NPC.NewNPC += On_NPC_NewNPC;
    }

    private int On_NPC_NewNPC(On_NPC.orig_NewNPC orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target) {
        if (Type == NPCID.FlyingAntlion && Main.rand.NextBool(20)) {
            Type = ModContent.NPCType<AlbinoSwarmer>();
        }

        return orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
    }

    public override void SetStaticDefaults() {
        NPC.SetTrail(6);

        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.FlyingAntlion];

        NPCID.Sets.NPCBestiaryDrawModifiers value = new() {
            Position = new Vector2(0f, -8f),
            PortraitPositionYOverride = -30f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

        BestiaryText = this.GetLocalization("Bestiary");
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        bestiaryEntry.Info.AddRange([BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                                     new FlavorTextBestiaryInfoElement(BestiaryText.ToString())]);
    }

    public override void SetDefaults() {
        NPC.CloneDefaults(NPCID.FlyingAntlion);

        AIType = NPCID.FlyingAntlion;
        AnimationType = NPCID.FlyingAntlion;

        NPC.rarity = 1;

        Banner = Type;
        BannerItem = ModContent.ItemType<Items.Placeable.Banners.AlbinoSwarmerBanner>();

        float scale = 1.25f;
        NPC.damage = (int)((float)NPC.damage * scale);
        NPC.defense = (int)((float)NPC.defense * scale);
        NPC.lifeMax = (int)((float)NPC.lifeMax * scale);
        NPC.value = (int)(NPC.value * scale);
    }

    //public override void AI() {
    //    if (!(NPC.shimmerTransparency > 0f)) {
    //        if (Main.rand.Next(1) == 0) {
    //            SoundEngine.PlaySound(Main.rand.NextFromList([SoundID.Zombie44, SoundID.Zombie45, SoundID.Zombie46]), NPC.Center);
    //        }
    //    }
    //}

    public override void HitEffect(NPC.HitInfo hit) {
        float num503 = 100f;
        float num504 = 50f;

        if (NPC.life > 0) {
            for (int num505 = 0; (double)num505 < hit.Damage / (double)NPC.lifeMax * (double)num503; num505++) {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 250, hit.HitDirection, -1f);
            }

            return;
        }

        for (int num506 = 0; (float)num506 < num504; num506++) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, 250, 2 * hit.HitDirection, -2f);
        }

        if (Main.dedServ) {
            return;
        }

        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoSwarmerGore1").Type);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoSwarmerGore2").Type);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoSwarmerGore3").Type);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoSwarmerGore4").Type);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("Consolaria/AlbinoSwarmerGore4").Type);
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot) {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BleachCanister>(), 10));
        npcLoot.Add(ItemDropRule.Common(3772, 50));
        npcLoot.Add(ItemDropRule.Common(323, 3, 1, 2));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.AlbinoMandible>(), 25));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        NPC rCurrentNPC = NPC;
        SpriteBatch mySpriteBatch = spriteBatch;
        int type = NPC.type;
        int[] npcFrameCount = Main.npcFrameCount;

        SpriteEffects spriteEffects = (-NPC.spriteDirection).ToSpriteEffects();

        Color npcColor = drawColor;
        npcColor = NPC.GetNPCColorTintedByBuffs(npcColor);

        float num35 = 0f;
        float num36 = Main.NPCAddHeight(rCurrentNPC);

        Vector2 halfSize = new Vector2(TextureAssets.Npc[type].Width() / 2, TextureAssets.Npc[type].Height() / Main.npcFrameCount[type] / 2);

        Texture2D value76 = TextureAssets.Npc[type].Value;
        Microsoft.Xna.Framework.Color color46 = Microsoft.Xna.Framework.Color.White;
        float amount10 = 0f;
        float amount11 = 0f;
        int num270 = 0;
        int num271 = 0;
        int num272 = 1;
        int num273 = 15;
        int num274 = 0;
        float scale8 = rCurrentNPC.scale;
        float value77 = rCurrentNPC.scale;
        int num275 = 0;
        float num276 = 0f;
        float num277 = 0f;
        float num278 = 0f;
        Microsoft.Xna.Framework.Color color47 = npcColor;
        Vector2 origin23 = halfSize;

        num270 = 6;
        num271 = 2;
        num273 = num270 * 3;

        num35 = -6f;

        for (int num292 = num272; num292 < num270; num292 += num271) {
            _ = ref rCurrentNPC.oldPos[num292];
            Microsoft.Xna.Framework.Color value79 = color47;
            value79 = Microsoft.Xna.Framework.Color.Lerp(value79, color46, amount10);
            value79 = rCurrentNPC.GetAlpha(value79);
            value79 *= (float)(num270 - num292) / (float)num273;
            _ = rCurrentNPC.rotation;
            if (num274 == 1)
                _ = rCurrentNPC.oldRot[num292];

            float scale9 = MathHelper.Lerp(scale8, value77, 1f - (float)(num270 - num292) / (float)num273);
            Vector2 position32 = rCurrentNPC.oldPos[num292] + new Vector2(rCurrentNPC.width, rCurrentNPC.height) / 2f - screenPos;
            position32 -= new Vector2(value76.Width, value76.Height / npcFrameCount[type]) * rCurrentNPC.scale / 2f;
            position32 += halfSize * rCurrentNPC.scale + new Vector2(0f, num35 + num36 + rCurrentNPC.gfxOffY);
            mySpriteBatch.Draw(value76, position32, rCurrentNPC.frame, value79, rCurrentNPC.rotation, halfSize, scale9, spriteEffects, 0f);
        }

        for (int num293 = 0; num293 < num275; num293++) {
            Microsoft.Xna.Framework.Color value80 = npcColor;
            value80 = Microsoft.Xna.Framework.Color.Lerp(value80, color46, amount10);
            value80 = rCurrentNPC.GetAlpha(value80);
            value80 = Microsoft.Xna.Framework.Color.Lerp(value80, color46, amount11);
            value80 *= 1f - num276;
            Vector2 position33 = rCurrentNPC.Center + ((float)num293 / (float)num275 * ((float)Math.PI * 2f) + rCurrentNPC.rotation + num278).ToRotationVector2() * num277 * num276 - screenPos;
            position33 -= new Vector2(value76.Width, value76.Height / npcFrameCount[type]) * rCurrentNPC.scale / 2f;
            position33 += halfSize * rCurrentNPC.scale + new Vector2(0f, num35 + num36 + rCurrentNPC.gfxOffY);
            mySpriteBatch.Draw(value76, position33, rCurrentNPC.frame, value80, rCurrentNPC.rotation, origin23, rCurrentNPC.scale, spriteEffects, 0f);
        }

        Vector2 vector70 = rCurrentNPC.Center - screenPos;
        vector70 -= new Vector2(value76.Width, value76.Height / npcFrameCount[type]) * rCurrentNPC.scale / 2f;
        vector70 += halfSize * rCurrentNPC.scale + new Vector2(0f, num35 + num36 + rCurrentNPC.gfxOffY);

        mySpriteBatch.Draw(value76, vector70, rCurrentNPC.frame, rCurrentNPC.GetAlpha(color47), rCurrentNPC.rotation, origin23, rCurrentNPC.scale, spriteEffects, 0f);

        return false;
    }
}
