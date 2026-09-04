using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    public ref float InitValue => ref NPC.ai[0];

    public bool Init {
        get => InitValue != 0f;
        set => InitValue = value.ToInt();
    }

    public override bool PreAI() {
        return base.PreAI();
    }

    public override void AI() {
        void init() {
            if (!Init) {
                Init = true;


            }
        }

        init();
    }

    public override void PostAI() {
        
    }
}
