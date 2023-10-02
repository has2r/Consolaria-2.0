using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace Consolaria.Content.NPCs.Bosses {
    public abstract class ConsolariaModBoss : ModNPC {
        protected short FrameWidth {
            get; set;
        }

        protected short FrameHeight {
            get; set;
        }

        protected int FrameIndex {
            get; set;
        }

        public override void SetStaticDefaults () {
            NPCID.Sets.TrailCacheLength [Type] = 8;
            NPCID.Sets.TrailingMode [Type] = 1;
        }

        public override void SetDefaults () {
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.scale = 1f;
            NPC.aiStyle = -1;
            NPC.npcSlots = 30f;

            AnimationType = -1;
        }

        public override bool CheckDead ()
            => false;

        public override bool CheckActive ()
            => false;

        public ref float StateTimer
            => ref NPC.ai [0];

        public ref float State
            => ref NPC.ai [1];

        protected void ChangeState (float newStateID) {
            State = newStateID;
            StateTimer = 0f;
        }

        protected void ChangeState (float newStateID, float newStateTimer) {
            State = newStateID;
            StateTimer = newStateTimer;
        }

        protected void SetFrame (int index) {
            SetFrame(index / Main.npcFrameCount [Type], index % Main.npcFrameCount [Type]);
            FrameIndex = index;
        }

        protected void SetFrame (int x, int y) {
            NPC.frame = new Rectangle(x * FrameWidth, y * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}