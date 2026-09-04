using Microsoft.Xna.Framework;
using System;
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
        void updateTime() {
            float smoothingFactor = 0.025f;
            float deltaTime = 1f / 60;
            if (Main.dayTime) {
                smoothingFactor *= 4;
                float to = (float)Main.dayLength;
                float t = 1f - MathF.Exp(-smoothingFactor * 60f * deltaTime);
                Main.time = MathHelper.Lerp((float)Main.time, to, t);
            }
            else {
                float to = (float)Main.nightLength / 2;
                float t = 1f - MathF.Exp(-smoothingFactor * 60f * deltaTime);
                Main.time = MathHelper.Lerp((float)Main.time, to, t);
            }
        }

        init();
        updateTime();
    }

    public override void PostAI() {
        
    }
}
