using Consolaria.Content.Buffs;
using Consolaria.Content.Dusts;
using Consolaria.Content.EmoteBubbles;
using Consolaria.Content.Items.Vanity;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.IO;

using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Consolaria.Content.NPCs.Friendly.McMoneypants;

[AutoloadHead()]
public class McMoneypants : ModNPC {
    #region Fields
    public static LocalizedText BestiaryText { get; private set; }

    private const double DAY_TIME = 48600.0;

    private static string _lastName = null;

    private static double _timePassed;

    private static int ShimmerHeadIndex;

    private static Profiles.StackedNPCProfile _NPCProfile;
    #endregion

    #region Properties
    public List<string> Names { get; private set; }
        = new List<string>() { "Milburn",
                               "Scrooge",
                               "Patrick",
                               "Jordan",
                               "Benny",
                               "Carter",
                               "Ulysses",
                               "Thomas",
                               "Jeff",
                               "Mark",
                               "Bill",
                               "Gabe",
                               "Steve",
                               "Walter",
                               "Rich",
                               "George" };

    public static List<LocalizedText> Quotes { get; private set; }

    public static List<LocalizedText> QuotesWhenInvested { get; private set; }

    public static List<LocalizedText> QuotesOnButtonClickWhenFirstTimeInvested { get; private set; }

    public static List<LocalizedText> QuotesOnButtonClickWhenPlayerHasNoMoney { get; private set; }

    public static List<LocalizedText> QuotesOnButtonClickWhenAlreadyInvested { get; private set; }

    internal static bool SpawnCondition
        => !McMoneypantsWorldData.Travelled && Main.IsItDay() && Main.time >= McMoneypantsWorldData.SpawnTime && Main.time < DAY_TIME;

    internal static bool DespawnCondition
        => _timePassed >= (McMoneypantsWorldData.SomebodyInvested ? DAY_TIME / 2 : DAY_TIME);

    public override void Load() {
        ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
    }

    #endregion

    #region Defaults
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
        NPCID.Sets.ShimmerTownTransform[Type] = true;
        NPCID.Sets.NoTownNPCHappiness[id] = true;

        NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) {
            Velocity = 1f,
            Direction = -1
        };

        NPCID.Sets.NPCBestiaryDrawOffset.Add(id, value);

        _NPCProfile = new Profiles.StackedNPCProfile(
        new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
        new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
        );

        NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<McMoneyPantsEmote>();

        BestiaryText = this.GetLocalization("Bestiary");
        QuotesOnButtonClickWhenAlreadyInvested = new List<LocalizedText>() {
            this.GetLocalization("AlreadyInvested1"), this.GetLocalization("AlreadyInvested2"), this.GetLocalization("AlreadyInvested3"), this.GetLocalization("AlreadyInvested4") };
        QuotesOnButtonClickWhenPlayerHasNoMoney = new List<LocalizedText>() {
            this.GetLocalization("PlayerHasNoMoney1"), this.GetLocalization("PlayerHasNoMoney2"), this.GetLocalization("PlayerHasNoMoney3"), this.GetLocalization("PlayerHasNoMoney4") };
        QuotesOnButtonClickWhenFirstTimeInvested = new List<LocalizedText>() {
            this.GetLocalization("FirstTimeInvested1"), this.GetLocalization("FirstTimeInvested2"), this.GetLocalization("FirstTimeInvested3"), this.GetLocalization("FirstTimeInvested4") };
        QuotesWhenInvested = new List<LocalizedText>() {
            this.GetLocalization("WhenInvested1"), this.GetLocalization("WhenInvested2"), this.GetLocalization("WhenInvested3"), this.GetLocalization("WhenInvested4"), this.GetLocalization("WhenInvested5"), this.GetLocalization("WhenInvested6"), this.GetLocalization("WhenInvested7"), this.GetLocalization("WhenInvested8") };
        Quotes = new List<LocalizedText>() {
            this.GetLocalization("Quote1"), this.GetLocalization("Quote2"), this.GetLocalization("Quote3"), this.GetLocalization("Quote4"), this.GetLocalization("Quote5"), this.GetLocalization("Quote6"), this.GetLocalization("Quote7"), this.GetLocalization("Quote8"), this.GetLocalization("Quote9"), this.GetLocalization("Quote10") };
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
               new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
           });

    public override void ModifyNPCLoot(NPCLoot npcLoot)
        => npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrestigiousTopHat>()));

    #endregion

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
    public override void HitEffect(NPC.HitInfo hit) {
        OnHitDusts();
        SpawnGoresOnDeath();
    }

    private void OnHitDusts() {
        int dustAmount = NPC.life > 0 ? 1 : 5;
        for (int k = 0; k < dustAmount; k++) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
        }
    }

    private void SpawnGoresOnDeath() {
        if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
            string variant = "";
            if (NPC.IsShimmerVariant) {
                variant += "_Shimmer";
            }
            int headGore = Mod.Find<ModGore>($"McMoneypantsGore1{variant}").Type;
            int armGore = Mod.Find<ModGore>($"McMoneypantsGore2{variant}").Type;
            int legGore = Mod.Find<ModGore>($"McMoneypantsGore3{variant}").Type;
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
        }
    }
    #endregion

    #region Chatbox
    public override List<string> SetNPCNameList()
        => Names;

    public override string GetChat()
        => Main.LocalPlayer.GetModPlayer<McMoneypantsPlayerData>().PlayerInvested ? QuotesWhenInvested[Main.rand.Next(QuotesWhenInvested.Count)].ToString() : Quotes[Main.rand.Next(Quotes.Count - 2)].ToString();

    public override void SetChatButtons(ref string button, ref string button2) {
        McMoneypantsPlayerData modPlayer = Main.LocalPlayer.GetModPlayer<McMoneypantsPlayerData>();
        button = Language.GetTextValue("Mods.Consolaria.McMoneypantsButton1Text") + (!modPlayer.PlayerInvested ? $" ({Helper.GetPriceText(modPlayer.PlayerInvestPrice, true)})" : string.Empty);
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        => OnFirstButtonClick();
    #endregion

    #region Town NPC
    public override ITownNPCProfile TownNPCProfile()
        => _NPCProfile;

    public override bool CanTownNPCSpawn(int numTownNPCs)
        => false;

    public override bool CanGoToStatue(bool toKingStatue)
        => toKingStatue;

    public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
        damage = 20;
        knockback = 8f;
    }

    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
        cooldown = 12;
        randExtraCooldown = 20;
    }

    public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
        projType = ModContent.ProjectileType<McMoneypantsAttackProjectile>();
        attackDelay = 1;
    }

    public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
        multiplier = 6f;
        randomOffset = 1.5f;
    }
    #endregion

    #region Custom
    private bool DespawnNPC() {
        void IncreaseTimePassedValue() {
            _timePassed += Main.dayRate;
        }

        bool CheckConditions() {
            return DespawnCondition && !Helper.IsNPCOnScreen(NPC.Center) && (!SpawnCondition || McMoneypantsWorldData.InvestedNextTravel);
        }

        void NotifyDespawnInChat() {
            if (Main.netMode == NetmodeID.SinglePlayer) {
                string text = Language.GetTextValue("LegacyMisc.35", NPC.FullName).Replace(NPC.FullName.Replace(NPC.GivenName, string.Empty), " Mc MoneyPants");
                Main.NewText(text, 50, 125, 255);
            }
            else {
                NetworkText text = NetworkText.FromKey("LegacyMisc.35", NetworkText.FromKey("Mods.Consolaria.Others.NPCTitle", NetworkText.FromLiteral(NPC.GivenName), NetworkText.FromKey(Lang.GetNPCName(NPC.netID).Key)));
                ChatHelper.BroadcastChatMessage(text, new Color(50, 125, 255));
            }
        }

        void KillNPC() {
            McMoneypantsWorldData.Travelled = false;

            NPC.active = false;
            NPC.netSkip = -1;
            NPC.life = 0;
        }

        void SaveNPCName() {
            _lastName = NPC.GivenName;
        }

        void ResetTimePassedValue() {
            _timePassed = 0.0;
        }

        IncreaseTimePassedValue();

        if (CheckConditions()) {
            NotifyDespawnInChat();

            SaveNPCName();

            ResetTimePassedValue();

            ResetInvestedStatus(NPC);

            KillNPC();

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
                string text = Language.GetTextValue("Announcement.HasArrived", npc.FullName).Replace(npc.FullName.Replace(npc.GivenName, string.Empty), " Mc MoneyPants");
                Main.NewText(text, 50, 125, 255);
            }
            else {
                NetworkText text = NetworkText.FromKey("Announcement.HasArrived", NetworkText.FromKey("Mods.Consolaria.Others.NPCTitle", NetworkText.FromLiteral(npc.GivenName), NetworkText.FromKey(Lang.GetNPCName(npc.netID).Key)));
                ChatHelper.BroadcastChatMessage(text, new Color(50, 125, 255));
            }
        }

        void UpdateSpawnTime() {
            bool isMorning = Main.IsItDay() && Main.time == 0;
            if (isMorning) {
                bool shouldComeWhen = Main.rand.NextBool(McMoneypantsWorldData.ChanceToSpawn) || McMoneypantsWorldData.InvestedNextTravel;
                if (!isMoneypantsThere && shouldComeWhen) {
                    int minTime = 5400, maxTime = 8100;
                    McMoneypantsWorldData.Travelled = false;
                    McMoneypantsWorldData.SpawnTime = Helper.GetRandomSpawnTime(minTime, maxTime);
                }
                else {
                    McMoneypantsWorldData.SpawnTime = double.MaxValue;
                }
            }
        }

        string ChooseRandomName(List<string> names) {
            return names[Main.rand.Next(names.Count)];
        }

        void SpawnNPC() {
            if (!isMoneypantsThere && Helper.CanTownNPCSpawn(SpawnCondition)) {
                McMoneypantsWorldData.Travelled = true;

                int npc = NPC.NewNPC(Terraria.Entity.GetSource_TownSpawn(), Main.spawnTileX * 16, Main.spawnTileY * 16, thisNPCType, 1);

                NPC worldNPC = Main.npc[npc];
                worldNPC.homeless = true;
                worldNPC.direction = Main.spawnTileX >= WorldGen.bestX ? -1 : 1;
                worldNPC.netUpdate = true;

                if (McMoneypantsWorldData.DidntTravelYet) {
                    McMoneypantsWorldData.DidntTravelYet = false;
                }
                else {
                    McMoneypants modNPC = worldNPC.ModNPC as McMoneypants;
                    List<string> names = modNPC.Names;
                    string lastName = _lastName, name = lastName ?? ChooseRandomName(names);
                    worldNPC.GivenName = name;
                }

                NotifySpawnInChat(worldNPC);
            }
        }

        UpdateSpawnTime();
        SpawnNPC();
    }

    private void OnFirstButtonClick() {
        Player player = Main.LocalPlayer;
        McMoneypantsPlayerData modPlayer = player.GetModPlayer<McMoneypantsPlayerData>();

        if (modPlayer.PlayerInvested) {
            Main.npcChatText = QuotesOnButtonClickWhenAlreadyInvested[Main.rand.Next(QuotesOnButtonClickWhenAlreadyInvested.Count)].ToString();

            return;
        }
        if (!player.BuyItem(modPlayer.PlayerInvestPrice)) {
            Main.npcChatText = QuotesOnButtonClickWhenPlayerHasNoMoney[Main.rand.Next(QuotesOnButtonClickWhenPlayerHasNoMoney.Count)].ToString();

            return;
        }

        void AddBuff() {
            player.AddBuff(ModContent.BuffType<Fortuned>(), 1800 * 60);
        }

        void UpdateInvestInfo() {
            McMoneypantsWorldData.InvestedNextTravel = McMoneypantsWorldData.SomebodyInvested = true;

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
            SoundStyle style = new($"{nameof(Consolaria)}/Assets/Sounds/MoneyCash");
            SoundEngine.PlaySound(style, NPC.Center);
            SoundEngine.PlaySound(SoundID.Coins with { Pitch = -0.3f }, NPC.Center);
        }

        AddBuff();
        UpdateInvestInfo();

        Main.npcChatText = QuotesOnButtonClickWhenFirstTimeInvested[Main.rand.Next(QuotesOnButtonClickWhenFirstTimeInvested.Count)].ToString();

        if (Main.netMode != NetmodeID.Server) {
            SpawnDusts();
            PlaySound();
        }
    }

    private static void ResetInvestedStatus(NPC npc) {
        McMoneypantsPlayerData modPlayer = Main.LocalPlayer.GetModPlayer<McMoneypantsPlayerData>();
        if (modPlayer.PlayerInvested) {
            modPlayer.PlayerInvested = McMoneypantsWorldData.SomebodyInvested = false;

            return;
        }

        _lastName = null;

        McMoneypantsWorldData.InvestedNextTravel = false;

        modPlayer.PlayerInvestPrice = McMoneypantsPlayerData.startPrice;
    }
    #endregion
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
    internal static bool isGildedInvitationUsed;

    public static bool InvestedNextTravel { get; internal set; }
    public static bool SomebodyInvested { get; internal set; }
    public static bool Travelled { get; internal set; }
    public static bool DidntTravelYet { get; internal set; } = true;

    public static double SpawnTime { get; internal set; } = double.MaxValue;

    internal static int ChanceToSpawn { get; private set; }

    public override void OnWorldLoad()
        => ResetValues();

    public override void OnWorldUnload()
        => ResetValues();

    public override void SaveWorldData(TagCompound tag) {
        tag.Add("inviteIsUsed", isGildedInvitationUsed);

        tag.Add("isInvested", InvestedNextTravel);
        tag.Add("isInvested2", SomebodyInvested);
        tag.Add("travelled", Travelled);
        tag.Add("firstTimeTravelled", DidntTravelYet);

        tag.Add("spawnTime", SpawnTime);
    }

    public override void LoadWorldData(TagCompound tag) {
        isGildedInvitationUsed = tag.GetBool("inviteIsUsed");

        InvestedNextTravel = tag.GetBool("isInvested");
        SomebodyInvested = tag.GetBool("isInvested2");
        Travelled = tag.GetBool("travelled");
        DidntTravelYet = tag.GetBool("firstTimeTravelled");

        SpawnTime = tag.GetDouble("spawnTime");
    }

    public override void NetSend(BinaryWriter writer) {
        writer.Write(isGildedInvitationUsed);

        writer.Write(InvestedNextTravel);
        writer.Write(SomebodyInvested);
        writer.Write(Travelled);
        writer.Write(DidntTravelYet);

        writer.Write(SpawnTime);
    }

    public override void NetReceive(BinaryReader reader) {
        isGildedInvitationUsed = reader.ReadBoolean();

        InvestedNextTravel = reader.ReadBoolean();
        SomebodyInvested = reader.ReadBoolean();
        Travelled = reader.ReadBoolean();
        DidntTravelYet = reader.ReadBoolean();

        SpawnTime = reader.ReadDouble();
    }

    public override void PostUpdateTime() {
        if (!isGildedInvitationUsed) {
            return;
        }

        McMoneypants.SpawnNPCRandomly();

        void UpdateSpawnToChance() {
            if (DidntTravelYet) {
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

    private static void ResetValues() {
        isGildedInvitationUsed = InvestedNextTravel = SomebodyInvested = false;

        DidntTravelYet = true;

        SpawnTime = double.MaxValue;

        ChanceToSpawn = 0;
    }
}
#endregion

#region NPC's Attack
public class McMoneypantsAttackProjectile : ModProjectile {
    private Vector2 _extraVelocity = Vector2.Zero;
    private float _glowTimer;

    public bool JustSpawned {
        get => Projectile.localAI[0] == 1f;
        set => Projectile.localAI[0] = value ? 1f : 0f;
    }

    public bool CanDamageEnemies {
        get => Projectile.localAI[1] == 1f;
        set => Projectile.localAI[1] = value ? 1f : 0f;
    }

    public bool Collided {
        get => Projectile.ai[0] == 1f;
        set => Projectile.ai[0] = value ? 1f : 0f;
    }

    public override void SetStaticDefaults()
        => Main.projFrames[Type] = 8;

    public override void SetDefaults() {
        Projectile.friendly = true;
        Projectile.penetrate = 1;

        int width = 12, height = 16;
        Projectile.Size = new Vector2(width, height);

        Projectile.timeLeft = 1200;
        Projectile.ignoreWater = true;

        DrawOriginOffsetY = 2;
    }

    public override bool PreAI() {
        if (!JustSpawned) {
            JustSpawned = true;

            Projectile.velocity.Y -= 5f;
        }

        return true;
    }

    public override void PostAI() {
        if (++Projectile.frameCounter > 4) {
            Projectile.frameCounter = 1;
            if (++Projectile.frame > Main.projFrames[Type]) {
                Projectile.frame = 1;
            }
        }
    }

    public override void AI() {
        CanDamageEnemies = Projectile.timeLeft < 1100;

        Player owner = Main.player[Projectile.owner];
        Helper.SearchForTargets(Projectile, owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);

        bool isMoving = false;

        Projectile.rotation += Projectile.velocity.Length() * (!foundTarget ? 0.05f : 0.2f);

        if (Collided) {
            Projectile.velocity *= 0.8f;

            if (CanDamageEnemies) {
                isMoving = MoveSlowlyToClosestTarget(foundTarget, distanceFromTarget, targetCenter);

                _glowTimer += 0.375f * (isMoving ? 0.5f : -1f);
            }
        }
        if (!isMoving) {
            Projectile.velocity.Y += 0.6f;
            if (Projectile.velocity.Y > 16f) {
                Projectile.velocity.Y = 16f;
            }
        }
    }

    public override bool? CanDamage()
        => CanDamageEnemies;

    private bool MoveSlowlyToClosestTarget(bool foundTarget, float distanceFromTarget, Vector2 targetCenter) {
        float speed = 6f;
        float inertia = 11f;

        if (foundTarget) {
            if (distanceFromTarget < 150f) {
                Vector2 direction = targetCenter - Projectile.Center;
                direction.Normalize();
                direction *= speed;

                _extraVelocity = (_extraVelocity * (inertia - 1) + direction) / inertia;
                Projectile.velocity += _extraVelocity;
                _extraVelocity = Vector2.Zero;

                return true;
            }
        }

        return false;
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        Collided = true;

        return false;
    }

    private class Explosion : ModProjectile {
        public override string Texture
            => "Consolaria/Assets/Textures/Empty";

        public override void SetDefaults() {
            Projectile.ignoreWater = true;

            Projectile.friendly = true;

            Projectile.timeLeft = 2;

            Projectile.Size = new Vector2(100, 100);
        }
    }

    public override void OnKill(int timeLeft) {
        if (Main.myPlayer == Projectile.owner) {
            Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Explosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }


        for (int i = 0; i < 10; i++) {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
            Main.dust[dustIndex].velocity *= 1.4f;
        }
        for (int i = 0; i < 10; i++) {
            int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
            Main.dust[dustIndex].noGravity = true;
            Main.dust[dustIndex].velocity *= 5f;
            dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
            Main.dust[dustIndex].velocity *= 3f;
        }
        int goreIndex = Gore.NewGore(Projectile.GetSource_FromAI(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
        Main.gore[goreIndex].scale = 1.5f;
        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
        goreIndex = Gore.NewGore(Projectile.GetSource_FromAI(), new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
        Main.gore[goreIndex].scale = 1.5f;
        Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
        Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
    }

    public override bool PreDraw(ref Color lightColor) {
        Texture2D bloomTex = ModContent.Request<Texture2D>("Consolaria/Assets/Textures/GlowAlpha").Value;
        Color bloomColor = Color.Lerp(Color.Transparent, new Color(255, 50, 15, 0), _glowTimer);
        for (int i = 0; i < 3; i++) {
            Main.spriteBatch.Draw(bloomTex, Projectile.Center - Main.screenPosition, null, bloomColor, Projectile.rotation, bloomTex.Size() / 2f, 0.4f, 0, 0f);
        }

        return true;
    }
}
#endregion