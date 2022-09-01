using Terraria.ModLoader;

namespace Consolaria.Content.Items.Pets {
    public abstract class PetItem : ModItem {
        public override bool CanRightClick () => false;
    }
}