using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    private static float ROTATIONLERP => 0.5f;

    private interface IAIState {
        public void OnActiveUpdate(NPC npc, EternalHorror boss);

        public void OnStart(NPC npc, EternalHorror boss) { }
        public void OnEnd(NPC npc, EternalHorror boss) { }
    }

    private readonly struct MoveToPlayer : IAIState {
        void IAIState.OnActiveUpdate(NPC npc, EternalHorror boss) {
            boss.TargetPlayer();

            Player target = npc.GetTargetPlayer();
            Vector2 targetCenter = target.Center,
                    baseTargetCenter = targetCenter;

            const int MinDistanceToTargetInPixels = 300;

            void lookAtTarget() {
                float angleToTarget = npc.AngleTo(baseTargetCenter) - MathHelper.PiOver2;
                npc.rotation = npc.rotation.AngleLerp(angleToTarget, ROTATIONLERP);
            }
            void makeTargetPositionABitHigher() {
                targetCenter.Y -= 100f;
            }
            void moveToTarget() {
                const float Speed = 15f,
                            Inertia = 20f;
                const float Deceleration = 0.99f;
                npc.MoveToWithDeceleration(targetCenter, Speed, Inertia, MinDistanceToTargetInPixels, Deceleration);

                Vector2 targetPosition = Vector2.Zero.MoveTowards(targetCenter - npc.Center, 4f);
                npc.velocity = npc.velocity.MoveTowards(targetPosition, 2f / 15f);
            }
            void moveFromTargetIfClose() {
                float distance = npc.Distance(targetCenter);
                float minDistance = MinDistanceToTargetInPixels / 2f;
                if (distance < minDistance) {
                    npc.velocity += npc.DirectionFrom(targetCenter) * (0.25f + 0.75f * Helper.Clamp01(1f - distance / minDistance));
                }
                else {
                    if (npc.velocity.Length() < 1f) {
                        npc.velocity *= 0.95f;
                    }
                }
            }
            void moveUpwardsIfClose() {
                if (npc.Center.Y > targetCenter.Y) {
                    npc.velocity -= Vector2.UnitY * 0.5f;
                }
            }
            void slowDownWhenCloseToTarget() {
                npc.velocity *= Helper.Clamp01(npc.Distance(targetCenter) / (MinDistanceToTargetInPixels / 5f));
            }

            lookAtTarget();
            makeTargetPositionABitHigher();
            moveToTarget();
            moveFromTargetIfClose();
            moveUpwardsIfClose();
            slowDownWhenCloseToTarget();
        }
    }

    private readonly struct Phase1LaserAttack : IAIState {
        public static float LASERATTACKTIME => Helper.SecondsToFrames(1);
        public static byte LASERATTACKCOUNT => 3;

        void IAIState.OnActiveUpdate(NPC npc, EternalHorror boss) {
            Player target = npc.GetTargetPlayer();

            float laserProgress = boss.Phase1LaserAttackProgress;
            float laserProgress_ForLaserRotation = laserProgress * Utils.GetLerpValue(1f, 0.5f, laserProgress, true);

            void shootLasers() {
                if ((boss.AICounter < 0f && laserProgress > 0f) || boss.AICounter >= LASERATTACKTIME * 0.5f) {
                    if (boss.AICounter % 2 == 0) {
                        const float Speed = 12f;
                        Vector2 vector8 = npc.Center;
                        SoundEngine.PlaySound(SoundID.Item33, vector8);
                        float rotation = vector8.AngleTo(target.Center + target.velocity * Speed / 2f);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, MathF.Cos(rotation) * Speed, MathF.Sin(rotation) * Speed, ModContent.ProjectileType<EternalHorrorLaser1>(),
                            27, 1.5f);
                    }
                }
            }
            void prepareLasers() {
                ref float laserRotation = ref boss.Phase1LaserRotation;
                float newLaserRotation = npc.AngleTo(target.Center);
                laserRotation = Utils.AngleLerp(laserRotation, newLaserRotation, laserProgress_ForLaserRotation);

                bool shotLaser = ++boss.AICounter >= LASERATTACKTIME;
                if (!shotLaser) {
                    return;
                }

                boss.ResetPhase1LaserAttack();

                bool shotLasers = ++boss.AttackCount >= LASERATTACKCOUNT;
                if (!shotLasers) {
                    return;
                }

                boss.ResetCounters();
                boss.DeactivateState<Phase1LaserAttack>();
                boss.ActivateState<Phase1ShadowSpawn>();
            }

            prepareLasers();
            shootLasers();
        }
    }

    private readonly struct Phase1ShadowSpawn : IAIState {
        void IAIState.OnActiveUpdate(NPC npc, EternalHorror boss) {
            void prepareClone() {
                float lerpValue = 1 / 60f;
                lerpValue *= 1.5f;
                npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, lerpValue);
                npc.rotation = npc.rotation.AngleLerp(npc.velocity.Length() * npc.direction, lerpValue);
            }

            prepareClone();
        }

        void IAIState.OnStart(NPC npc, EternalHorror boss) {
            boss.DeactivateState<MoveToPlayer>();
        }

        void IAIState.OnEnd(NPC npc, EternalHorror boss) {
            boss.ActivateState<MoveToPlayer>();
        }
    }

    private Dictionary<Type, IAIState> _states = null!;
    private HashSet<IAIState> _activeStates = null!;

    public ref float Phase1LaserRotation => ref NPC.ai[2];

    public float Phase1LaserAttackProgress => Helper.Clamp01(AICounter / Phase1LaserAttack.LASERATTACKTIME);

    private void InitializeStates() {
        _states = [];
        _activeStates = [];
    }

    private void AddState<T>() where T : struct, IAIState => _states.TryAdd(typeof(T), new T());

    private void ActivateState<T>() where T : struct, IAIState {
        AddState<T>();
        IAIState stateToActivate = _states[typeof(T)];
        if (!_activeStates.Contains(stateToActivate)) {
            stateToActivate.OnStart(npc: NPC, boss: Self);
        }
        else {
            return;
        }
        _activeStates.Add(stateToActivate);
    }

    private void DeactivateState<T>() where T : IAIState {
        IAIState stateToDeactivate = _states[typeof(T)];
        if (!_activeStates.Contains(stateToDeactivate)) {
            return;
        }
        stateToDeactivate.OnEnd(npc: NPC, boss: Self);
        _activeStates.Remove(stateToDeactivate);
    }

    private bool HasActiveState<T>() where T : IAIState {
        if (_states.TryGetValue(typeof(T), out IAIState state)) {
            return _activeStates.Contains(state);
        }
        return false;
    }
}
