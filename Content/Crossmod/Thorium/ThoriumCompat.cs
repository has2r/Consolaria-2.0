
using Consolaria.Content.Crossmod.Thorium.Buffs;
using Consolaria.Content.Crossmod.Thorium.Dusts;
using Consolaria.Content.Crossmod.Thorium.Projectiles;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

using ThoriumMod;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.EarlyMagic;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.NPCs;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Projectiles.Healer;
using ThoriumMod.Projectiles.Scythe;
using ThoriumMod.Projectiles.Thrower;
using ThoriumMod.Utilities;

namespace Consolaria.Content.Crossmod.Thorium;

public sealed class ThoriumCompat : ModSystem {
    public const string THORIUMMODNAME = "ThoriumMod";

    public static Mod ThoriumMod {
        get {
            if (ModLoader.TryGetMod(THORIUMMODNAME, out Mod thoriumMod)) {
                return thoriumMod;
            }
            return null;
        }
    }

    public static bool IsThoriumEnabled => ThoriumMod != null;
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public sealed class ThoriumNPC_Consolaria : GlobalNPC {
    public override void UpdateLifeRegen(NPC npc, ref int damage) {
        if (ThoriumPlayer_Consolaria.AnyViperSetNearby(npc.Center)) {
            if (npc.venom) {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= (int)(60 * ThoriumPlayer_Consolaria.VIPEREFFECTDAMAGEINCREASE);
                int minDamage = (int)(15 * ThoriumPlayer_Consolaria.VIPEREFFECTDAMAGEINCREASE);
                if (damage < minDamage)
                    damage = minDamage;
            }
            if (npc.poisoned) {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= (int)(12 * ThoriumPlayer_Consolaria.VIPEREFFECTDAMAGEINCREASE);
            }
        }
    }

    public override void DrawEffects(NPC npc, ref Color drawColor) {
        if (!npc.active) {
            return;
        }
        if (!npc.canDisplayBuffs) {
            return;
        }
        if (ThoriumPlayer_Consolaria.AnyViperSetNearby(npc.Center)) {
            if (npc.venom || npc.poisoned) {
                if (Main.rand.Next(20) == 0) {
                    Dust dust2 = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<ViperDust>(), 0f, 0f, 125, default(Color), Main.rand.NextFloat(0.5f, 0.75f));
                    dust2.position.Y -= npc.height / 2;
                    dust2.noGravity = true;
                    dust2.fadeIn = 1.5f;
                }
            }
        }
    }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public sealed class ThoriumPlayer_Consolaria : ModPlayer {
    private static ushort SIRENSPAWNSEACREATURECOOLDOWN => Helper.SecondsToFrames(5);
    private static ushort SERAPHIMEFFECTTIME => Helper.SecondsToFrames(4);
    private static ushort SERAPHIMEFFECTCOOLDOWN => Helper.SecondsToFrames(30);
    private static float SERAPHIMMOVESPEEDBOOSTMODIFIER => 2f;

    private static ushort VIPEREFFECTDISTANCE => (ushort)(Helper.TILESIZE * 20);
    public static float VIPEREFFECTDAMAGEINCREASE => 1.5f;

    public static Color SERAPHIMGLOWCOLOR => Color.Gold with { A = 100 };

    public double PressedSpecial;

    public bool IsSeraphimSetBonusActive, IsSeraphimSetBonusActive2;
    public int SeraphimFlightTime;
    public float SeraphimEffectOpacity;

    public int HealedBySeraphim_HealerWhoAmI = -1;

    public bool IsViperSetBonusActive;

    public bool IsSirenSetBonusActive;
    public ushort CanSpawnSirenSeaCreatureCounter;

    public bool IsSeraphimEffectActive => SeraphimFlightTime > 0;
    public bool IsSeraphimEffectActive2 => IsSeraphimSetBonusActive2 && IsSeraphimEffectActive;
    public bool IsSeraphimEffectOnCooldown => Player.HasBuff<SeraphimCooldown>();
    public bool CanSpawnSirenSeaCreature => IsSirenSetBonusActive && CanSpawnSirenSeaCreatureCounter <= 0;

    public override void Load() {
        On_Player.UpdateJumpHeight += On_Player_UpdateJumpHeight;
    }

    private void On_Player_UpdateJumpHeight(On_Player.orig_UpdateJumpHeight orig, Player self) {
        orig(self);
        if (!self.mount.Active && self.GetModPlayer<ThoriumPlayer_Consolaria>().IsSeraphimEffectActive2) {
            self.jumpSpeedBoost += 1.8f * SERAPHIMMOVESPEEDBOOSTMODIFIER;
        }
    }

    public override void PostUpdateRunSpeeds() {
        if (IsSeraphimEffectActive2) {
            Player.runAcceleration *= 1.75f * SERAPHIMMOVESPEEDBOOSTMODIFIER;
        }
    }

    public override void ResetEffects() {
        IsSeraphimSetBonusActive = false;
        IsViperSetBonusActive = false;
        IsSirenSetBonusActive = false;
    }

    public static bool AnyViperSetNearby(Vector2 checkPosition) {
        bool result = false;
        foreach (Player player in Main.ActivePlayers) {
            if (player.Distance(checkPosition) > VIPEREFFECTDISTANCE) {
                continue;
            }
            if (player.GetModPlayer<ThoriumPlayer_Consolaria>().IsViperSetBonusActive) {
                result = true;
                break;
            }
        }
        return result;
    }

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
        if (!IsViperSetBonusActive) {
            return;
        }

        if (!item.CountsAsClass(DamageClass.Throwing)) {
            return;
        }

        if (Main.rand.NextBool(10)) {
            target.AddBuff(BuffID.Venom, Helper.SecondsToFrames(Main.rand.Next(2, 5)));
        }
        if (Main.rand.NextBool(10)) {
            target.AddBuff(BuffID.Poisoned, Helper.SecondsToFrames(Main.rand.Next(2, 5)));
        }
    }

    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
        //if (!target.CanBeChasedBy()) {
        //    return;
        //}

        TriggerViperSetBonus(proj, target, hit, damageDone);
        TriggerSirenSetBonus(proj, target, hit, damageDone);
    }

    private void TriggerSirenSetBonus(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
        if (!target.CanBeChasedBy()) {
            return;
        }

        if (!proj.CountsAsClass(ThoriumDamageBase<BardDamage>.Instance)) {
            return;
        }

        SpawnSirenCreature(proj.GetSource_OnHit(target));
    }

    private void TriggerViperSetBonus(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
        if (!IsViperSetBonusActive) {
            return;
        }

        if (!proj.CountsAsClass(DamageClass.Throwing)) {
            return;
        }

        if (Main.rand.NextBool(10)) {
            target.AddBuff(BuffID.Venom, Helper.SecondsToFrames(Main.rand.Next(2, 5)));
        }
        if (Main.rand.NextBool(10)) {
            target.AddBuff(BuffID.Poisoned, Helper.SecondsToFrames(Main.rand.Next(2, 5)));
        }
    }

    public override void PostUpdateEquips() {
        UpdateSeraphimSetBonus();

        UpdateSirenSetBonus();
    }

    private void UpdateSirenSetBonus() {
        if (CanSpawnSirenSeaCreatureCounter > 0) {
            CanSpawnSirenSeaCreatureCounter--;
        }
    }

    public void SpawnSirenCreature(IEntitySource source_OnHit) {
        if (!CanSpawnSirenSeaCreature) {
            return;
        }

        if (Player.IsLocal()) {
            List<int> types = [1, 2, 3, 4, 5, 6];
            int count = types.Count;
            int i = Main.rand.Next(count);
            float maxX = 300f;
            float x = MathHelper.Lerp(-maxX, maxX, (float)i / count) + maxX / count;
            Vector2 position = Player.GetPlayerCorePoint() + new Vector2(x, 300f);
            Projectile.NewProjectileDirect(source_OnHit,
                                           position,
                                           Vector2.Zero,
                                           ModContent.ProjectileType<SirenSeaCreature>(),
                                           0, 0,
                                           Player.whoAmI,
                                           ai0: x,
                                           ai2: types.TakeRandom() - 1);
        }

        CanSpawnSirenSeaCreatureCounter = SIRENSPAWNSEACREATURECOOLDOWN;
    }

    private void UpdateSeraphimSetBonus() {
        if (IsSeraphimEffectActive2) {
            SeraphimFlightTime--;
            if (SeraphimFlightTime <= 0) {
                IsSeraphimSetBonusActive2 = false;

                Player.AddBuff<SeraphimCooldown>(SERAPHIMEFFECTCOOLDOWN);
                SeraphimFlightTime = -SERAPHIMEFFECTCOOLDOWN;
            }

            ApplySeraphimEffect();
        }
        if (SeraphimFlightTime < 0) {
            SeraphimFlightTime++;
            if (!IsSeraphimEffectOnCooldown) {
                SeraphimFlightTime = 0;
            }
        }
        float to = IsSeraphimEffectActive.ToInt();
        float lerpValue = 0.1f;
        if (to < 1f) {
            lerpValue /= 2;
        }
        SeraphimEffectOpacity = Helper.Approach(SeraphimEffectOpacity, to, lerpValue);
        Lighting.AddLight(Player.GetPlayerCorePoint(), SERAPHIMGLOWCOLOR.ToVector3() * 1f * SeraphimEffectOpacity);
    }

    public override void TransformDrawData(ref PlayerDrawSet drawInfo) {
        ApplySeraphimGlow(ref drawInfo);
    }

    private void ApplySeraphimGlow(ref PlayerDrawSet drawInfo) {
        int count = drawInfo.DrawDataCache.Count;
        if (SeraphimEffectOpacity > 0f) {
            for (int i = 0; i < count; i++) {
                DrawData value = drawInfo.DrawDataCache[i];
                value.color = Color.Lerp(value.color, drawInfo.drawPlayer.GetImmuneAlphaPure(SERAPHIMGLOWCOLOR, drawInfo.shadow), SeraphimEffectOpacity * 0.875f);
                drawInfo.DrawDataCache[i] = value;
            }
        }
    }

    private void ApplySeraphimEffect() {
        Player.wingTime = Player.wingTimeMax;
        Player.rocketTime = Player.rocketTimeMax;
        Player.moveSpeed += 0.075f * SERAPHIMMOVESPEEDBOOSTMODIFIER;
    }

    public override void ProcessTriggers(TriggersSet triggersSet) {
        TriggerArmorKeybind();
    }

    private void TriggerArmorKeybind() {
        if (Player.CCed) {
            return;
        }

        ThoriumPlayer handler = Player.GetModPlayer<ThoriumPlayer>();
        bool pressedSpecial = Math.Abs(handler.ThoriumTime - PressedSpecial) > 30.0;
        if (ThoriumHotkeySystem.ArmorKey.JustPressed && pressedSpecial) {
            PressedSpecial = handler.ThoriumTime;
            OnArmorKeyPressed();
        }
    }

    private void OnArmorKeyPressed() {
        ActivateSeraphimEffect();
    }

    private void ActivateSeraphimEffect() {
        if (IsSeraphimEffectOnCooldown) {
            return;
        }
        if (IsSeraphimSetBonusActive) {
            if (!IsSeraphimSetBonusActive2) {
                SeraphimFlightTime = SERAPHIMEFFECTTIME;

                Player.AddBuff<SeraphimBuff>(SERAPHIMEFFECTTIME);

                IsSeraphimSetBonusActive2 = true;

                SoundEngine.PlaySound(new SoundStyle("ThoriumMod/Sounds/Item/Pulse"), Player.Center);

                if (Player.IsLocal()) {
                    Projectile.NewProjectileDirect(Player.GetSource_FromThis(),
                                                   Player.GetPlayerCorePoint(),
                                                   Vector2.Zero,
                                                   ModContent.ProjectileType<SeraphimHeal>(),
                                                   0, 0,
                                                   Player.whoAmI);
                }
            }
        }
    }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumBuff_Base : ModBuff {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumItem_BardBase : BardItem {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumItem_HealerBase : ThoriumItem {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;

    public virtual bool IsDarkHealer { get; } = false;

    public sealed override void SetDefaults() {
        isHealer = true;

        bool isAHealerTool = false;
        SetHealerValues(ref isDarkHealer, ref healType, ref healAmount, ref healDisplay, ref isAHealerTool);
        if (isDarkHealer) {
            isHealer = false;
        }

        SetHealerDefaults();

        if (isAHealerTool) {
            Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
        }
    }

    public virtual void SetHealerDefaults() { }

    public virtual void SetHealerValues(ref bool IsDarkHealer, ref HealType healType, ref int healAmount, ref bool healDisplay, ref bool isAHealerTool) { }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumItem_ThrowerBase : ThoriumItem {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;

    public sealed override void SetDefaults() {
        isThrower = true;

        SetThrowerValues(ref isThrowerNon, ref isThrowerNeedle, ref isThrowerTomahawk, ref isThrowerCaltrop);

        SetThrowerDefaults();

        if (Item.IsAWeapon()) {
            Item.DamageType = DamageClass.Throwing;
        }
    }

    public virtual void SetThrowerDefaults() { }

    public virtual void SetThrowerValues(ref bool isThrowerNon, ref bool IsThrowerNeedle, ref bool IsThrowerTomahawk, ref bool IsThrowerCaltrop) { }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumItem_ScytheBase : ScytheItem {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;

    public sealed override void SetStaticDefaults() {
        SetStaticDefaultsToScythe();

        SetScytheStaticDefaults();
    }

    public virtual void SetScytheStaticDefaults() { }

    public sealed override void SetDefaults() {
        SetDefaultsToScythe();

        SetScytheValues(ref scytheSoulCharge);

        SetScytheDefaults();
    }

    public virtual void SetScytheDefaults() { }

    public virtual void SetScytheValues(ref int scytheSoulCharge) { }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumProjectile_BardBase : BardProjectile {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumProjectile_ScytheBase : ScythePro {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;

    public sealed override void SafeSetDefaults() {
        SetScytheValues(ref dustCount, ref dustType, ref dustOffset);

        SetScytheDefaults();
    }

    public virtual void SetScytheDefaults() { }

    public virtual void SetScytheValues(ref int dustCount, ref int dustType, ref Vector2 dustOffset) { }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumProjectile_HealerBase : ThoriumProjectile {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;

    public sealed override void SetDefaults() {
        if (Projectile.IsAWeapon()) {
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
        }

        SetHealerDefaults();
    }

    public virtual void SetHealerDefaults() { }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumProjectile_ThrowerBase : ThoriumProjectile {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;

    public sealed override void SetDefaults() {
        Projectile.DamageType = DamageClass.Throwing;

        SetThrowerDefaults();
    }

    public virtual void SetThrowerDefaults() { }
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public abstract class ThoriumProjectile_TomahawkBase : TomahawkProBase {
    public override bool IsLoadingEnabled(Mod mod) => ThoriumCompat.IsThoriumEnabled;
}

[ExtendsFromMod(ThoriumCompat.THORIUMMODNAME)]
[JITWhenModsEnabled(ThoriumCompat.THORIUMMODNAME)]
public static class ThoriumUtils {
    public delegate void CustomHealing(Player player, Player target, ref int heals, ref int selfHeals);

    public static bool ThoriumHealTarget(this Projectile projectile, Player target, int healAmount, bool onHealEffects = true, bool bonusHealing = true, bool ignoreSetTarget = false, bool statistics = true, CustomHealing customHealing = null) {
        //IL_0206: Unknown result type (might be due to invalid IL or missing references)
        //IL_0211: Unknown result type (might be due to invalid IL or missing references)
        //IL_0194: Unknown result type (might be due to invalid IL or missing references)
        //IL_019f: Unknown result type (might be due to invalid IL or missing references)
        if (projectile.owner != Main.myPlayer) {
            return false;
        }
        Player player = Main.player[projectile.owner];
        ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
        ThoriumPlayer thoriumTarget = target.GetThoriumPlayer();
        int heals = healAmount;
        int selfHeals = 0;
        bool self = player == target;
        customHealing?.Invoke(player, target, ref heals, ref selfHeals);
        if (bonusHealing) {
            heals += thoriumPlayer.healBonus;
        }
        if (onHealEffects) {
            if (thoriumPlayer.accForgottenCrossNecklace) {
                target.AddBuff(ModContent.BuffType<ForgottenCrossNecklaceBuff>(), 900, false, false);
            }
            if (thoriumPlayer.setBlooming) {
                target.AddBuff(ModContent.BuffType<BloomingSetBuff>(), 600, false, false);
            }
            if (thoriumPlayer.setLifeBinder) {
                target.AddBuff(ModContent.BuffType<LifeBinderSetBuff>(), 600, false, false);
            }
            if (thoriumPlayer.accVerdantOrnament) {
                target.AddBuff(ModContent.BuffType<VerdantOrnamentBuff>(), 300, false, false);
            }
            if (thoriumPlayer.buffDreamWeaversHoodDream) {
                target.AddBuff(ModContent.BuffType<DreamWeaversHoodDreamAllyBuff>(), 60, false, false);
            }
            if (!self && thoriumPlayer.honeyHeart && target.statLife <= player.statLife) {
                target.AddPVPBuff(48, 300);
            }
            ref int coralShieldCounter = ref thoriumPlayer.setCoralShieldCounter;
            if (!self && target != player && thoriumPlayer.setCoral && coralShieldCounter > 0) {
                thoriumPlayer.HandleCoralSetTransfer(thoriumTarget, coralShieldCounter, request: true);
                coralShieldCounter = 0;
            }
            if (!self && thoriumPlayer.accBeltoftheQuickResponse) {
                player.AddBuff(ModContent.BuffType<BeltoftheQuickResponseBuff>(), 180, false, false);
            }
            if (!self && thoriumPlayer.prydwen) {
                selfHeals += 4;
            }
            if (!self && thoriumPlayer.innerFlame.Active && thoriumPlayer.LowestPlayer != ((Entity)player).whoAmI) {
                Projectile.NewProjectile(player.GetSource_Accessory(thoriumPlayer.innerFlame.Item, (string)null), ((Entity)player).Center.X, ((Entity)player).Center.Y - 50f, 0f, 0f, ModContent.ProjectileType<InnerFlamePro>(), 0, 0f, ((Entity)player).whoAmI, 0f, 0f, 0f);
            }
            if (!self && thoriumPlayer.accDewCollector.Active) {
                Projectile.NewProjectile(player.GetSource_Accessory(thoriumPlayer.accDewCollector.Item, (string)null), ((Entity)target).Center.X, ((Entity)target).Center.Y, Utils.NextFloat(Main.rand, -1f, 1f), Utils.NextFloat(Main.rand, -3f, -1f), ModContent.ProjectileType<DewCollectorPro>(), 0, 0f, ((Entity)player).whoAmI, 0f, 0f, 0f);
            }
            if (thoriumPlayer.aloePlant) {
                thoriumTarget.SetLifeRecoveryEffect(LifeRecoveryEffectType.AloeLeaf, 600, request: true);
            }
            if (thoriumPlayer.medicalAcc && !thoriumTarget.OutOfCombat) {
                thoriumTarget.SetLifeRecoveryEffect(LifeRecoveryEffectType.MedicalBag, 300, request: true);
            }
            if (!self && thoriumPlayer.equilibrium) {
                ((player.statLife > target.statLife) ? thoriumTarget : thoriumPlayer).SetLifeRecoveryEffect(LifeRecoveryEffectType.Equalizer, 300, request: true);
            }
        }
        if (heals > 0) {
            target.HealLife(heals, player, healOverMax: true, statistics);
            thoriumTarget.mostRecentHeal = heals;
            thoriumTarget.mostRecentHealer = ((Entity)player).whoAmI;
            if (!ignoreSetTarget) {
                thoriumPlayer.healedTarget = ((Entity)target).whoAmI;
            }
            player.ApplyInteractionNearbyNPCs();
        }
        if (selfHeals > 0) {
            player.HealLife(selfHeals);
        }
        if (projectile.penetrate > 0) {
            projectile.penetrate--;
        }
        if (projectile.penetrate == 0) {
            projectile.Kill();
            return true;
        }
        return false;
    }

    public static bool CanBeHealed(this Projectile projectile, Player healer, Player target, float radius = 0f, Func<Player, bool> canHealPlayer = null, int specificPlayer = -1, bool ignoreHealer = true) {
        //IL_0084: Unknown result type (might be due to invalid IL or missing references)
        bool isSpecificPlayer = specificPlayer > -1 && specificPlayer < 255;
        if (!((Entity)target).active || target.dead || target.statLife >= target.statLifeMax2 || (!isSpecificPlayer && ignoreHealer && ((Entity)healer).whoAmI == ((Entity)target).whoAmI) || (isSpecificPlayer && specificPlayer != ((Entity)target).whoAmI) || (canHealPlayer != null && !canHealPlayer(target)) || (healer.team != target.team && healer.team != 0)) {
            return false;
        }
        if (radius != 0f && ((Entity)target).DistanceSQ(((Entity)projectile).Center) > radius * radius) {
            return false;
        }
        return true;
    }

    public static void ThoriumHeal(this Projectile projectile, int healAmount, float radius = 30f, bool onHealEffects = true, bool bonusHealing = true, CustomHealing customHealing = null, Func<Player, bool> canHealPlayer = null, int specificPlayer = -1, bool ignoreHealer = true, bool ignoreSetTarget = false, bool statistics = true) {
        //IL_00e6: Unknown result type (might be due to invalid IL or missing references)
        if (projectile.owner != Main.myPlayer) {
            return;
        }
        Player healer = Main.player[projectile.owner];
        if (specificPlayer > -1 && specificPlayer < 255) {
            Player target = Main.player[specificPlayer];
            if (projectile.CanBeHealed(healer, target, radius, canHealPlayer, specificPlayer, ignoreHealer)) {
                projectile.ThoriumHealTarget(target, healAmount, onHealEffects, bonusHealing, ignoreSetTarget, statistics, customHealing);
            }
        }
        else {
            for (int i = 0; i < 255; i++) {
                Player target = Main.player[i];
                if (projectile.CanBeHealed(healer, target, radius, canHealPlayer, specificPlayer, ignoreHealer) && projectile.ThoriumHealTarget(target, healAmount, onHealEffects, bonusHealing, ignoreSetTarget, statistics, customHealing)) {
                    break;
                }
            }
        }
        if (projectile.penetrate == 0) {
            return;
        }
        ThoriumPlayer thoriumPlayer = healer.GetThoriumPlayer();
        int dummyType = ModContent.NPCType<HealingDummy>();
        for (int u = 0; u < Main.maxNPCs; u++) {
            NPC dummy = Main.npc[u];
            if (((Entity)dummy).active && dummy.type == dummyType && !(((Entity)dummy).DistanceSQ(((Entity)projectile).Center) > radius * radius)) {
                int heals = healAmount;
                if (bonusHealing) {
                    heals += thoriumPlayer.healBonus;
                }
                dummy.life += heals;
                dummy.HealEffect(heals, true);
                if (dummy.localAI[0] <= 0f) {
                    dummy.localAI[0] = 300f;
                }
                if (projectile.penetrate > 0) {
                    projectile.penetrate--;
                }
                if (projectile.penetrate == 0) {
                    projectile.Kill();
                    break;
                }
            }
        }
    }

    public static bool CanHitLine(this Entity start, Entity end) {
        return CanHitLine(Utils.ToTileCoordinates(start.Center), Utils.ToTileCoordinates(end.Center));
    }

    public static bool CanHitLine(Vector2 start, Vector2 end) {
        return CanHitLine(Utils.ToTileCoordinates(start), Utils.ToTileCoordinates(end));
    }

    public static bool CanHitLine(Point start, Point end) {
        if (!WorldGen.InWorld(start.X, start.Y, 0) || !WorldGen.InWorld(end.X, end.Y, 0) || WorldGen.SolidTile3(Framing.GetTileSafely(start))) {
            return false;
        }
        int distX = Math.Abs(end.X - start.X);
        int distY = Math.Abs(end.Y - start.Y);
        int sign_x = ((end.X - start.X > 0) ? 1 : (-1));
        int sign_y = ((end.Y - start.Y > 0) ? 1 : (-1));
        int ix = 0;
        int iy = 0;
        while (ix < distX || iy < distY) {
            int xyDiff = ((1 + 2 * ix) * distY).CompareTo((1 + 2 * iy) * distX);
            if (xyDiff == 0) {
                if (WorldGen.SolidTile3(Framing.GetTileSafely(start.X + sign_x, start.Y)) || WorldGen.SolidTile3(Framing.GetTileSafely(start.X, start.Y + sign_y))) {
                    return false;
                }
                start.X += sign_x;
                start.Y += sign_y;
                ix++;
                iy++;
            }
            else if (xyDiff < 0) {
                start.X += sign_x;
                ix++;
            }
            else {
                start.Y += sign_y;
                iy++;
            }
            if (WorldGen.SolidTile3(Framing.GetTileSafely(start))) {
                return false;
            }
        }
        return true;
    }

    public static NPC FindNearestNPC(this Projectile projectile, float maxRange, bool absoluteDistance = true, bool ignoreDontTakeDamage = false, Func<NPC, bool> isValidTarget = null, bool checkCollision = true) {
        NPC nearest = null;
        if (!absoluteDistance) {
            maxRange *= maxRange;
        }
        for (int i = 0; i < Main.maxNPCs; i++) {
            NPC npc = Main.npc[i];
            if (npc.CanBeChasedBy((object)projectile, ignoreDontTakeDamage) && (isValidTarget == null || isValidTarget(npc))) {
                float currentDistance = ((!absoluteDistance) ? ((Entity)projectile).DistanceSQ(((Entity)npc).Center) : (Math.Abs(((Entity)projectile).Center.X - ((Entity)npc).Center.X) + Math.Abs(((Entity)projectile).Center.Y - ((Entity)npc).Center.Y)));
                if (currentDistance < maxRange && (!checkCollision || ((Entity)(object)projectile).CanHitLine((Entity)(object)npc))) {
                    maxRange = currentDistance;
                    nearest = npc;
                }
            }
        }
        return nearest;
    }
}