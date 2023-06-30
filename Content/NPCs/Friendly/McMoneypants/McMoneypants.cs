using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Consolaria.Content.NPCs.Friendly.McMoneypants;

[AutoloadHead()]
public class McMoneypants : ModNPC {
    #region Fields
    private const double DAY_TIME = 48600.0;

    public const string BUTTON_TEXT = "Invest";

    private static double _timePassed;
    #endregion

    #region Properties
    public List<string> Names { get; private set; }
        = new List<string>() { "Ryan",                  //gosling
                               "Adolf",                 //um
                               "Alfred",
                               "Bernard",
                               "Charles",
                               "John",
                               "Jeff",
                               "Joseph",
                               "Jack",                  //jack ma
                               "Goldman",               //literally orwell
                               "Henry",
                               "Larry",
                               "Ludwig",
                               "Warren",
                               "William",
                               "Theodore",              //bombastic
                               "Savelii" };             //savelii andrusovich\
                               

    public List<string> Quotes { get; private set; } 
        = new List<string>() { /*"I want to be cremated, it's my last hope for a smoking hot body...",*/ //tf?!!
                               "To the guy who invented zero - thanks for nothing.",
                               "What was Forrest Gump’s email password? 1forrest1",
                               //"I was wondering why that ball was getting bigger. Then it hit me..",
                               "Waking up this morning was really an eye-opening experience.",
                               "Don't waste money on crypto - just buy my stocks!",
                               "I feel so clean, like a money machine!",
                               "Trust me, it's not a Ponzi Scheme.",
                               "Some say even Midas was wearing this suit.",
                               "What a beautiful day to make some money!",
                               "Ferragamo Gold!", //quote when invested and the button is clicked
                               "No money phrase" };

    public List<string> QuotesWhenInvested { get; private set; }
        = new List<string>() { "INVESTED!",
                               "INVEST! INVEST! INVEST!",
                               "Multiplying money in-progress...",
                               "MONEY!",
                               "PUMP & DUMP!" };

    internal static bool SpawnCondition
        => Main.dayTime && Main.time >= McMoneypantsWorldData.SpawnTime && Main.time < DAY_TIME;

    internal static bool DespawnCondition
        => _timePassed >= (McMoneypantsWorldData.SomebodyInvested ? DAY_TIME / 2 : DAY_TIME);
    #endregion

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

        NPCID.Sets.NoTownNPCHappiness[id] = true;

        NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)  {
			Velocity = 1f,
			Direction = -1
		};

	    NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
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
               BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
               new FlavorTextBestiaryInfoElement("WIP - to add late"),
           });

    //public override void OnSpawn(IEntitySource source)
    //    => ResetInvestedStatus();

    #region AI
    public override bool PreAI()
        => DespawnNPC();

    public override void AI()
        => NPC.homeless = true;

    public override void PostAI() {
    }
    #endregion

    #region Visuals
    public override void HitEffect(NPC.HitInfo hit)
        => OnHitDusts();

    private void OnHitDusts() {
        int dustAmount = NPC.life > 0 ? 1 : 5;
        for (int k = 0; k < dustAmount; k++) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
        }
    }
    #endregion

    #region Chatbox
    public override List<string> SetNPCNameList()
        => Names;

    public override string GetChat()
        => Main.LocalPlayer.GetModPlayer<McMoneypantsPlayerData>().PlayerInvested ? QuotesWhenInvested[Main.rand.Next(QuotesWhenInvested.Count)] : Quotes[Main.rand.Next(Quotes.Count - 2)];

    public override void SetChatButtons(ref string button, ref string button2) {
        McMoneypantsPlayerData modPlayer = Main.LocalPlayer.GetModPlayer<McMoneypantsPlayerData>();
        button = BUTTON_TEXT + (!modPlayer.PlayerInvested ? $" ({Helper.GetPriceText(modPlayer.PlayerInvestPrice)})" : string.Empty);
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        => OnFirstButtonClick();
    #endregion

    #region Town NPC
    public override bool CanTownNPCSpawn(int numTownNPCs)
        => false;

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
    #endregion

    private bool DespawnNPC() {
        void IncreaseTimePassedValue() {
            _timePassed += Main.dayRate;
        }

        bool CheckConditions() {
            return DespawnCondition && !Helper.IsNPCOnScreen(NPC.Center) && (!SpawnCondition || McMoneypantsWorldData.SomebodyInvested);
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
            NPC.active = false;
            NPC.netSkip = -1;
            NPC.life = 0;
        }

        void ResetTimePassedValue() {
            _timePassed = 0.0;
        }

        IncreaseTimePassedValue();

        if (CheckConditions())  {
            NotifyDespawnInChat();
            KillNPC();

            ResetTimePassedValue();

            ResetInvestedStatus();

            return false;
        }

        return true;
    }

    public static void SpawnNPCRandomly() {
        if (Main.netMode == NetmodeID.MultiplayerClient) {
            return;
        }

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
                bool shouldComeWhen = Main.rand.NextBool(McMoneypantsWorldData.ChanceToSpawn) || McMoneypantsWorldData.SomebodyInvested;
                if (!isMoneypantsThere && shouldComeWhen) {
                    int minTime = 5400, maxTime = 8100;
                    McMoneypantsWorldData.SpawnTime = Helper.GetRandomSpawnTime(minTime, maxTime);
                }
                else {
                    McMoneypantsWorldData.SpawnTime = double.MaxValue;
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

                McMoneypantsWorldData.FirstTimeTravelled = false;

                NotifySpawnInChat(worldNPC);
            }
        }

        UpdateSpawnTime();
        SpawnNPC();
    }

    private void OnFirstButtonClick() {
        void UpdateChatTextWhenInvested() {
            int lastElementIndex = Quotes.Count - 2;
            Main.npcChatText = Quotes[lastElementIndex];
        }

        void UpdateChatTextWhenNoMoney() {
            int lastElementIndex = Quotes.Count - 1;
            Main.npcChatText = Quotes[lastElementIndex];
        }

        Player player = Main.LocalPlayer;
        McMoneypantsPlayerData modPlayer = player.GetModPlayer<McMoneypantsPlayerData>();

        if (modPlayer.PlayerInvested) {
            UpdateChatTextWhenInvested();

            return;
        }
        if (!player.BuyItem(modPlayer.PlayerInvestPrice)) {
            UpdateChatTextWhenNoMoney();

            return;
        }

        void AddBuff() {
            player.AddBuff(ModContent.BuffType<McMoneypantsBuff>(), 1800 * 60);
        }

        void UpdateInvestInfo() {
            McMoneypantsWorldData.SomebodyInvested = true;

            modPlayer.PlayerInvested = true;
            modPlayer.PlayerInvestPrice += modPlayer.PlayerInvestPrice / 3;
            modPlayer.PlayerInvestPrice += Item.buyPrice(gold: 5);
        }

        void SpawnDusts() {
            Player player = Main.LocalPlayer;
            int width = player.width, height = player.height;

            int dustType = ModContent.DustType<MoneyDust>();
            for (int k = 0; k < Main.rand.Next(5, 8); k++) {
                Dust dust = Dust.NewDustPerfect(Main.LocalPlayer.Center + new Vector2(0f, (float)height * 0.2f) + Main.rand.NextVector2CircularEdge(width, (float)height * 0.6f) * (0.3f + Main.rand.NextFloat() * 0.5f), 
                                                dustType,
                                                new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.3f - 1.5f) * 0.5f, 
                                                Main.rand.Next(80, 130),
                                                default);
                dust.fadeIn = 1.1f;
            }
        }

        void PlaySound() {
            SoundStyle style = new($"{nameof(Consolaria)}/Assets/Sounds/MoneyCashSound");
            SoundEngine.PlaySound(style, NPC.Center);
        }

        AddBuff();
        UpdateInvestInfo();
        UpdateChatTextWhenInvested();

        if (Main.netMode != NetmodeID.Server) { 
            SpawnDusts();
            PlaySound();
        }
    }

    private static void ResetInvestedStatus() {
        McMoneypantsPlayerData modPlayer = Main.LocalPlayer.GetModPlayer<McMoneypantsPlayerData>();
        if (modPlayer.PlayerInvested) {
            modPlayer.PlayerInvested = false;

            return;
        }

        McMoneypantsWorldData.SomebodyInvested = false;

        modPlayer.PlayerInvestPrice = McMoneypantsPlayerData.startPrice;
    }
}

#region Data
public class McMoneypantsPlayerData : ModPlayer {
    internal static readonly int startPrice = Item.buyPrice(gold: 15);

    public long PlayerInvestPrice { get; internal set; } = startPrice;

    public bool PlayerInvested { get; internal set; } = false;

    public override void SaveData(TagCompound tag) {
        tag.Add("investPrice", PlayerInvestPrice);

        tag.Add("invested", PlayerInvested);
    }

    public override void LoadData(TagCompound tag) {
        PlayerInvestPrice = tag.GetLong("investPrice");

        PlayerInvested = tag.GetBool("invested");
    }
}

public class McMoneypantsWorldData : ModSystem {
    internal static bool GildedInvitationUsed;

    public static bool SomebodyInvested { get; internal set; }
    public static bool FirstTimeTravelled { get; internal set; } = true;

    public static double SpawnTime { get; internal set; } = double.MaxValue;

    internal static int ChanceToSpawn { get; private set; }

    public override void OnWorldLoad()
        => ResetValues();

    public override void OnWorldUnload()
        => ResetValues();

    public override void SaveWorldData(TagCompound tag) {
        tag.Add("isInvested", SomebodyInvested);
        tag.Add("firstTimeTravelled", FirstTimeTravelled);

        tag.Add("spawnTime", SpawnTime);

        tag.Add("inviteUsed", GildedInvitationUsed);
    }

    public override void LoadWorldData(TagCompound tag) {
        SomebodyInvested = tag.GetBool("isInvested");
        FirstTimeTravelled = tag.GetBool("firstTimeTravelled");

        SpawnTime = tag.GetDouble("spawnTime");

        GildedInvitationUsed = tag.GetBool("inviteUsed");
    }

    public override void NetSend(BinaryWriter writer) {
        writer.Write(SomebodyInvested);
        writer.Write(FirstTimeTravelled);

        writer.Write(SpawnTime);

        writer.Write(GildedInvitationUsed);
    }

    public override void NetReceive(BinaryReader reader) {
        SomebodyInvested = reader.ReadBoolean();
        FirstTimeTravelled = reader.ReadBoolean();

        SpawnTime = reader.ReadDouble();

        GildedInvitationUsed = reader.ReadBoolean();
    }

    public override void PostUpdateTime() {
        if (!GildedInvitationUsed) {
            return;
        }

        McMoneypants.SpawnNPCRandomly();

        void UpdateSpawnToChance() {
            if (FirstTimeTravelled) {
                ChanceToSpawn = 1;
            }
            else { 
                double rate = Main.dayRate;
                if (rate < 1.0) {
                    rate = 1.0;
                }
                ChanceToSpawn = (int)(27000.0 / rate);
                ChanceToSpawn *= 4;
            }
        }

        UpdateSpawnToChance();
    }

    private void ResetValues() {
        GildedInvitationUsed = SomebodyInvested = false;

        FirstTimeTravelled = true;

        SpawnTime = double.MaxValue;

        ChanceToSpawn = 0;
    }
}
#endregion