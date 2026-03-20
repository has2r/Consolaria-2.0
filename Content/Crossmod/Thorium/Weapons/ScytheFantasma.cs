using Microsoft.Xna.Framework;

using Terraria;

namespace Consolaria.Content.Crossmod.Thorium.Weapons;

public sealed class ScytheFantasma : ThoriumItem_ScytheBase {
    public override void SetScytheDefaults() {
        Item.SetSizeValues(64, 60);

        Item.SetShopValues(Terraria.Enums.ItemRarityColor.White0, Item.sellPrice());
        Item.SetShootableValues<ScytheFantasma_Use>();
    }

    public sealed class ScytheFantasma_Use : ThoriumProjectile_ScytheBase {
        public override void SetScytheDefaults() {
            Projectile.SetSizeValues(164, 156);
        }

        public override void SetScytheValues(ref int dustCount, ref int dustType, ref Vector2 dustOffset) {
            
        }
    }
}
