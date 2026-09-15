using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    private static byte CLONECOUNTAVAILABLE => 3;
    private static ushort CLONEACTIVETIME => Helper.SecondsToFrames(5);

    private static HashSet<CloneInfo> _cloneDataCache = [];

    private partial void Unload_Caches() {
        _cloneDataCache.Clear();
        _cloneDataCache = null;
    }

    public record struct CloneInfo(Vector2 Position, Vector2 TargetPosition, ushort TimeLeft, ushort MaxTimeLeft, float Rotation = 0f, Vector2 VisualPosition = default) {
        public readonly float TimeLeftProgress => (float)TimeLeft / MaxTimeLeft;
        public readonly bool Active => TimeLeftProgress > 0f;
        public readonly float Opacity {
            get {
                float timeLeftProgress = TimeLeftProgress;
                float opacity = 1f;
                opacity *= 1f - Utils.GetLerpValue(0.75f, 1f, timeLeftProgress, true);
                opacity *= Utils.GetLerpValue(0f, 0.25f, timeLeftProgress, true);
                return opacity;
            }
        }

        public readonly Vector2 GetFinalClonePosition(Player target) {
            Vector2 targetCenter = target.Center,
                    clonePosition = Position,
                    cloneTargetCenter = TargetPosition;
            Vector2 position = targetCenter;
            position += clonePosition - cloneTargetCenter;
            return position;
        }
    }

    private CloneInfo[] _cloneData = null!;

    public ref float InitValue => ref NPC.ai[0];

    public ref float AICounter => ref NPC.ai[1];

    public ref float AttackCount => ref NPC.localAI[3];
    public ref float SmoothFactor => ref NPC.localAI[2];
    public ref float Phase1LaserAttackCount => ref NPC.localAI[1];
    public ref float Phase1DashAttackCount => ref NPC.localAI[1];

    public bool Init {
        get => InitValue != 0f;
        set => InitValue = value.ToInt();
    }

    public override bool PreAI() => base.PreAI();

    public override void AI() {
        OnSpawn();
        MakeMidnight();
        UpdateStates();
        UpdateClones();
    }

    public override void PostAI() { }

    private void OnSpawn() {
        if (Init) {
            return;
        }

        Init = true;

        ResetPhase1LaserAttack(applyIncreasedDelay: true);

        TargetPlayer();

        SpawnFromAbove();

        InitializeClones();

        InitializeStates();

        ActivateState<MoveToPlayer>();
        ActivateState<Phase1LaserAttack>();
    }

    private void InitializeClones() {
        _cloneData = new CloneInfo[CLONECOUNTAVAILABLE];
    }

    private void SpawnClone() {
        int nextCloneAddedIndex = 0;
        for (int i = 0; i < _cloneData.Length; i++) {
            if (_cloneData[i].TimeLeft > 0) {
                nextCloneAddedIndex++;
            }
        }
        if (nextCloneAddedIndex >= CLONECOUNTAVAILABLE) {
            return;
        }
        if (!NPC.HasPlayerTarget) {
            return;
        }
        Player target = NPC.GetTargetPlayer();
        Vector2 npcCenter = NPC.Center,
                targetCenter = target.Center;
        Vector2 clonePosition = targetCenter + (targetCenter - npcCenter);
        ushort cloneActiveTime = CLONEACTIVETIME;
        _cloneData[nextCloneAddedIndex] = new CloneInfo(Position: clonePosition,
                                                        TargetPosition: targetCenter,
                                                        TimeLeft: cloneActiveTime,
                                                        MaxTimeLeft: cloneActiveTime,
                                                        VisualPosition: NPC.Center,
                                                        Rotation: NPC.rotation);
    }

    private partial void InitializeStates();

    private void MakeMidnight() {
        float expFactor = 0.025f;
        if (Main.dayTime) {
            expFactor *= 4;
            float to = (float)Main.dayLength;
            float lerpValue = 1f - MathF.Exp(-expFactor);
            Main.time = MathHelper.Lerp((float)Main.time, to, lerpValue);
        }
        else {
            float to = (float)Main.nightLength / 2;
            float lerpValue = 1f - MathF.Exp(-expFactor);
            Main.time = MathHelper.Lerp((float)Main.time, to, lerpValue);
        }
    }

    private void UpdateStates() {
        IAIState[] states = [.. _activeStates];
        foreach (IAIState activeState in states) {
            activeState.OnActiveUpdate(npc: NPC, boss: Self);
        }
    }

    private void UpdateClones() {
        for (int i = 0; i < _cloneData.Length; i++) {
            ref CloneInfo cloneInfo = ref _cloneData[i];
            if (cloneInfo.TimeLeft > 0) {
                cloneInfo.TimeLeft--;
            }

            Player target = NPC.GetTargetPlayer();
            cloneInfo.VisualPosition = Vector2.Lerp(cloneInfo.VisualPosition, cloneInfo.GetFinalClonePosition(target), 0.125f);

            Vector2 targetCenter = target.Center,
                    clonePosition = cloneInfo.VisualPosition;
            float angleToTarget = clonePosition.AngleTo(targetCenter) - MathHelper.PiOver2;
            cloneInfo.Rotation = cloneInfo.Rotation.AngleLerp(angleToTarget, ROTATIONLERP);

        }
    }

    public HashSet<CloneInfo> GetActiveCloneData() {
        _cloneDataCache.Clear();
        HashSet<CloneInfo> clonePositions = _cloneDataCache;
        if (!Init) {
            return clonePositions;
        }
        foreach (CloneInfo cloneInfo in _cloneData) {
            if (!cloneInfo.Active) {
                continue;
            }

            clonePositions.Add(cloneInfo);
        }

        return clonePositions;
    }

    private void TargetPlayer() {
        if (NPC.ShouldTargetPlayer()) {
            NPC.TargetClosest(faceTarget: false);
        }
    }

    private void SpawnFromAbove() {
        Vector2 spawnOffset = new(0f, -850f);
        NPC.Center = NPC.GetTargetPlayer().Center + spawnOffset;
    }

    private void ResetPhase1LaserAttack(bool applyIncreasedDelay = false) {
        if (applyIncreasedDelay) {
            AICounter = -(int)(Phase1LaserAttack.LASERATTACKTIME / 1f);
            return;
        }
        AICounter = -(int)(Phase1LaserAttack.LASERATTACKTIME / 2f);
    }

    private void ResetCounters() {
        AttackCount = 0;
        AICounter = 0f;
    }
}
