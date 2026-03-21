
using Microsoft.Xna.Framework;

using Terraria.ModLoader;

using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Scythe;
using ThoriumMod.Projectiles.Thrower;

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

        SetHealerValues(ref isDarkHealer);
        if (isDarkHealer) {
            isHealer = false;
        }

        SetHealerDefaults();
    }

    public virtual void SetHealerDefaults() { }

    public virtual void SetHealerValues(ref bool IsDarkHealer) { }
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
        Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;

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