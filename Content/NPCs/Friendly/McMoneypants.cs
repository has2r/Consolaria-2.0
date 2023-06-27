using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader.IO;

using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;

namespace Consolaria.Content.NPCs.Friendly;

[AutoloadHead()]
public class McMoneypants : ModNPC {
    private static double _spawnTime = double.MaxValue;

    public List<string> Names { get; private set; }
        = new List<string>() { "Ryan Gosling",
                               "Adolf Hitler",
                               "Savelii Andrusovich" };

    public List<string> Quotes { get; private set; } 
        = new List<string>() { "I want to be cremated as it is my last hope for a smoking hot body.",
                               "To the guy who invented zero, thanks for nothing.",
                               "What was Forrest Gump’s email password? 1forrest1",
                               "I was wondering why the ball was getting bigger. Then it hit me.",
                               "Waking up this morning was an eye-opening experience.",
                               "INVESTED!!!" };

    internal static bool SpawnCondition
        => Main.dayTime && Main.time >= _spawnTime && Main.time < 48600.0;

    public const string BUTTON_TEXT = "Invest";

    public override void SetStaticDefaults() {
        int id = Type;

        Main.npcFrameCount[id] = 25;

        NPCID.Sets.ExtraFramesCount[id] = 9;

        NPCID.Sets.DangerDetectRange[id] = 400;

        NPCID.Sets.AttackFrameCount[id] = 4;
        NPCID.Sets.AttackType[id] = 0;
        NPCID.Sets.AttackTime[id] = 10;
        NPCID.Sets.AttackAverageChance[id] = 10;

        NPCID.Sets.HatOffsetY[id] = 0;
    }

    public override void SetDefaults() {
        NPC.townNPC = true;
        TownNPCStayingHomeless = true;

        NPC.friendly = true;

        NPC.width = 18;
        NPC.height = 40;

        NPC.aiStyle = 7;

        NPC.damage = 10;
        NPC.defense = 15;
        NPC.lifeMax = 250;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        NPC.knockBackResist = 0.5f;

        AnimationType = NPCID.Guide;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        => bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] { 
               BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
           });

    public override bool PreAI()
        => DespawnNPC();

    private bool DespawnNPC() {
        bool CheckConditions() {
            return !Helper.IsNPCOnScreen(NPC.Center) && (!SpawnCondition || McMoneypantsWorldData.SomebodyInvested);
        }

        void NotifyDespawnInChat() {
            if (Main.netMode == NetmodeID.SinglePlayer) {
                Main.NewText(Language.GetTextValue("LegacyMisc.35", NPC.FullName), 50, 125, 255);
            }
            else {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("LegacyMisc.35", NPC.GetFullNetName()), new Color(50, 125, 255));
            }
        }

        void KillNPC() {
            Main.NewText(1);

            NPC.active = false;
            NPC.netSkip = -1;
            NPC.life = 0;

            ResetInvestedStatus();
        }

        if (CheckConditions())  {
            NotifyDespawnInChat();
            KillNPC();
            return false;
        }
        return true;
    }

    public override void AI()
        => NPC.homeless = true;

    public override void HitEffect(NPC.HitInfo hit)
        => OnHitDusts();

    private void OnHitDusts() {
        int dustAmount = NPC.life > 0 ? 1 : 5;
        for (int k = 0; k < dustAmount; k++) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain);
        }
    }

    public override bool CanTownNPCSpawn(int numTownNPCs)
        => false;

    public override List<string> SetNPCNameList()
        => Names;

    public override string GetChat()
        => Quotes[Main.rand.Next(Quotes.Count - 1)];

    public override void SetChatButtons(ref string button, ref string button2) {
        McMoneypantPlayerData modPlayer = Main.LocalPlayer.GetModPlayer<McMoneypantPlayerData>();
        button = BUTTON_TEXT + (!modPlayer.PlayerInvested ? $" ({Helper.GetPriceText(modPlayer.PlayerInvestPrice)})" : string.Empty);
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        => OnClick();

    public override bool CanGoToStatue(bool toKingStatue) 
        => !toKingStatue;

    public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
        damage = 20;
        knockback = 8f;
    }

    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
        cooldown = 12;
        randExtraCooldown = 20;
    }

    public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
        projType = ProjectileID.GoldCoin;
        attackDelay = 1;
    }

    public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
        multiplier = 6f;
        randomOffset = 1.5f;
    }

    public static void SpawnNPCRandomly() {
        int thisNPCType = ModContent.NPCType<McMoneypants>();
        bool isMoneypantsThere = NPC.FindFirstNPC(thisNPCType) != -1;

        void NotifySpawnInChat(NPC npc) {
            if (Main.netMode == NetmodeID.SinglePlayer) {
                Main.NewText(Language.GetTextValue("Announcement.HasArrived", npc.FullName), 50, 125, 255);
            }
            else {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasArrived", npc.GetFullNetName()), new Color(50, 125, 255));
            }
        }

        void UpdateSpawnTime() {
            bool isMorning = Main.dayTime && Main.time == 0;
            if (isMorning)  {
                bool shouldComeWhen = Main.rand.NextChance(McMoneypantsWorldData.ChanceToSpawn) || McMoneypantsWorldData.SomebodyInvested;
                if (!isMoneypantsThere && shouldComeWhen) {
                    _spawnTime = Helper.GetRandomSpawnTime(5400, 8100);
                }
                else {
                    _spawnTime = double.MaxValue;
                }
            }
        }

        void SpawnNPC() {
            if (!isMoneypantsThere && Helper.CanTownNPCSpawn(SpawnCondition)) {
                int npc = NPC.NewNPC(Terraria.Entity.GetSource_TownSpawn(), Main.spawnTileX * 16, Main.spawnTileY * 16, thisNPCType, 1);
                NPC worldNPC = Main.npc[npc];
                worldNPC.homeless = true;
                worldNPC.direction = Main.spawnTileX >= WorldGen.bestX ? -1 : 1;
                worldNPC.netUpdate = true;

                _spawnTime = double.MaxValue;

                NotifySpawnInChat(worldNPC);

                ResetInvestedStatus();
            }
        }

        UpdateSpawnTime();
        SpawnNPC();
    }

    private void OnClick() {
        McMoneypantPlayerData modPlayer = Main.LocalPlayer.GetModPlayer<McMoneypantPlayerData>();
        if (modPlayer.PlayerInvested) {
            return;
        }

        McMoneypantsWorldData.SomebodyInvested = true;

        modPlayer.PlayerInvested = true;
        modPlayer.PlayerInvestPrice += modPlayer.PlayerInvestPrice / 3;
        modPlayer.PlayerInvestPrice += Item.buyPrice(gold: 5);

        int lastElementIndex = Quotes.Count - 1;
        Main.npcChatText = Quotes[lastElementIndex];
    }

    private static void ResetInvestedStatus() {
        McMoneypantPlayerData modPlayer = Main.LocalPlayer.GetModPlayer<McMoneypantPlayerData>();
        if (modPlayer.PlayerInvested) {
            modPlayer.PlayerInvested = false;
            return;
        }

        McMoneypantsWorldData.SomebodyInvested = false;
    }
}

public class McMoneypantPlayerData : ModPlayer {
    private static readonly int _startPrice = Item.buyPrice(gold: 15);

    public int PlayerInvestPrice { get; internal set; } = _startPrice;

    public bool PlayerInvested { get; internal set; } = false;

    public override void SaveData(TagCompound tag) {
        tag.Add("investPrice", PlayerInvestPrice);

        tag.Add("invested", PlayerInvested);
    }

    public override void LoadData(TagCompound tag) {
        PlayerInvestPrice = tag.GetInt("investPrice");

        PlayerInvested = tag.GetBool("invested");
    }
}

public class McMoneypantsWorldData : ModSystem {
    public static bool SomebodyInvested { get; internal set; }

    internal static double ChanceToSpawn { get; private set; } = 1f;

    public override void SaveWorldData(TagCompound tag)
        => tag.Add("isInvested", SomebodyInvested);

    public override void LoadWorldData(TagCompound tag) 
        => SomebodyInvested = tag.GetBool("isInvested");

    public override void NetSend(BinaryWriter writer)
        => writer.Write(SomebodyInvested);

    public override void NetReceive(BinaryReader reader)
        => SomebodyInvested = reader.ReadBoolean();

    public override void PostUpdateTime()
        => McMoneypants.SpawnNPCRandomly();
}