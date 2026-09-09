using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    public override bool PreAI() => base.PreAI();

    public override void AI() {
        OnSpawn();
        MakeMidnight();
        UpdateStates();
    }

    public override void PostAI() { }

    private void OnSpawn() {
        if (Init) {
            return;
        }

        Init = true;

        _states = [];

        AddState<IdleState>();

        ChangeState<IdleState>();
    }

    private void MakeMidnight() {
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

    private void UpdateStates() {
        _activeState.OnActiveUpdate(Self);
    }
}
