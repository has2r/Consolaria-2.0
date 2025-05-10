using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.Crossmod.RoA;

sealed class RoACompat : ModSystem {
    private static Mod _riseOfAges;

    internal static Mod RiseOfAges {
        get {
            if (_riseOfAges == null && ModLoader.TryGetMod("RoA", out Mod mod)) {
                _riseOfAges = mod;
            }
            return _riseOfAges;
        }
    }

    public static bool IsRoAEnabled => RiseOfAges != null;

    public override void Unload() => _riseOfAges = null;

    internal static void MakeItemNature(Item item) => RiseOfAges?.Call("MakeItemNature", item);
    internal static void MakeItemDruidicWeapon(Item item) => RiseOfAges?.Call("MakeItemDruidicWeapon", item);
    internal static void SetDruidicWeaponValues(Item item, ushort potentialDamage, float fillingRateModifier = 1f) {
        MakeItemDruidicWeapon(item);
        RiseOfAges?.Call("SetDruidicWeaponValues", item, potentialDamage, fillingRateModifier);
    }

    internal static ushort GetDruidicWeaponBaseDamage(Item item, Player player) => (ushort)RiseOfAges?.Call("GetDruidicWeaponBaseDamage", item, player);
    internal static ushort GetDruidicWeaponBasePotentialDamage(Item item, Player player) => (ushort)RiseOfAges?.Call("GetDruidicWeaponBasePotentialDamage", item, player);
    internal static ushort GetDruidicWeaponCurrentDamage(Item item, Player player) => (ushort)RiseOfAges?.Call("GetDruidicWeaponCurrentDamage", item, player);

    internal static void MakeProjectileDruidic(Projectile projectile) => RiseOfAges?.Call("MakeProjectileDruidic", projectile);
    internal static void SetDruidicProjectileValues(Projectile projectile, bool shouldChargeWreathOnDamage = true, bool shouldApplyAttachedNatureWeaponCurrentDamage = true, float wreathFillingFine = 0f) {
        MakeProjectileDruidic(projectile);
        RiseOfAges?.Call("SetDruidicProjectileValues", projectile, shouldChargeWreathOnDamage, shouldApplyAttachedNatureWeaponCurrentDamage, wreathFillingFine);
    }

    internal static void SetAttachedNatureWeaponToDruidicProjectile(Projectile projectile, Item item) => RiseOfAges?.Call("SetAttachedNatureWeaponToDruidicProjectile", projectile, item);
    internal static Item GetAttachedNatureWeaponToDruidicProjectile(Projectile projectile) => (Item)RiseOfAges?.Call("GetAttachedNatureWeaponToDruidicProjectile", projectile);
}
