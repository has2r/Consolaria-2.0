using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    private partial void SetBestiaryInfo() {
        NPCID.Sets.BossBestiaryPriority.Add(Type);
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        
    }
}
