using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    public ref float InitValue => ref NPC.ai[0];

    public ref float AICounter => ref NPC.ai[1];

    public bool Init {
        get => InitValue != 0f;
        set => InitValue = value.ToInt();
    }

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

        TargetPlayer();

        SpawnFromAbove();

        InitializeStates();

        ActivateState<MoveToPlayer>();
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
        foreach (IAIState activeState in _activeStates) {
            activeState.OnActiveUpdate(NPC, Self);
        }
    }

    private void TargetPlayer() {
        if (NPC.ShouldTargetPlayer()) {
            NPC.TargetClosest(false);
        }
    }

    private void SpawnFromAbove() {
        Vector2 spawnOffset = new(0f, -1150f);
        NPC.Center = NPC.GetTargetPlayer().Center + spawnOffset;
    }
}
