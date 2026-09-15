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

            bool shouldDash = boss.HasActiveState<Phase1DashAttack>();

            Player target = npc.GetTargetPlayer();
            Vector2 targetCenter = target.Center,
                    baseTargetCenter = targetCenter;

            const int MinDistanceToTargetInPixels = 300;

            void smoothEverything() {
                if (shouldDash) {
                    return;
                }
                boss.SmoothFactor = Helper.Approach(boss.SmoothFactor, 1f, 0.025f);
            }
            void lookAtTarget() {
                if (shouldDash) {
                    return;
                }
                float angleToTarget = npc.AngleTo(baseTargetCenter) - MathHelper.PiOver2;
                npc.rotation = npc.rotation.AngleLerp(angleToTarget, ROTATIONLERP * boss.SmoothFactor);
            }
            void makeTargetPositionABitHigher() {
                if (shouldDash) {
                    return;
                }
                targetCenter.Y -= 100f;
            }
            void moveToTarget() {
                const float Speed = 15f,
                            Inertia = 20f;
                const float Deceleration = 0.99f;
                npc.MoveToWithDeceleration(targetCenter, Speed * boss.SmoothFactor, Inertia * boss.SmoothFactor, MinDistanceToTargetInPixels, Deceleration);

                Vector2 targetPosition = Vector2.Zero.MoveTowards(targetCenter - npc.Center, 4f * boss.SmoothFactor);
                npc.velocity = npc.velocity.MoveTowards(targetPosition, 2f / 15f * boss.SmoothFactor);
            }
            void moveFromTargetIfClose() {
                if (shouldDash) {
                    return;
                }
                float distance = npc.Distance(targetCenter);
                float minDistance = MinDistanceToTargetInPixels / 2f;
                if (distance < minDistance) {
                    npc.velocity += npc.DirectionFrom(targetCenter) * (0.25f + 0.75f * Helper.Clamp01(1f - distance / minDistance)) * boss.SmoothFactor;
                }
                else {
                    if (npc.velocity.Length() < 1f) {
                        npc.velocity *= 0.95f;
                    }
                }
            }
            void moveUpwardsIfClose() {
                if (shouldDash) {
                    return;
                }
                if (npc.Center.Y > targetCenter.Y) {
                    npc.velocity -= Vector2.UnitY * 0.5f * boss.SmoothFactor;
                }
            }
            void slowDownWhenCloseToTarget() {
                if (shouldDash) {
                    return;
                }
                npc.velocity *= Helper.Clamp01(npc.Distance(targetCenter) / (MinDistanceToTargetInPixels / 5f));
            }

            smoothEverything();
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

                boss.Phase1LaserAttackCount++;

                bool shotLasers = ++boss.AttackCount >= LASERATTACKCOUNT;
                if (!shotLasers) {
                    return;
                }

                boss.ResetCounters();
                boss.DeactivateState<Phase1LaserAttack>();

                bool shouldDash = boss.Phase1LaserAttackCount > Phase1DashAttack.LASERATTACKCOUNTNEEDED;
                if (shouldDash) {
                    boss.ActivateState<Phase1DashAttack>();

                    boss.Phase1LaserAttackCount = 0;

                    return;
                }

                boss.ActivateState<Phase1ShadowSpawn>();
            }

            prepareLasers();
            shootLasers();
        }
    }

    private readonly struct Phase1ShadowSpawn : IAIState {
        public static float SHADOWSPAWNTIME => Helper.SecondsToFrames(1);

        void IAIState.OnActiveUpdate(NPC npc, EternalHorror boss) {
            void prepareClone() {
                //float lerpValue = 1 / 60f;
                //lerpValue *= 1.5f;
                //npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, lerpValue);
                //npc.rotation = npc.rotation.AngleLerp(npc.velocity.Length() * npc.direction, lerpValue);

                bool justStarted = boss.AICounter == 0f;
                if (justStarted) {
                    boss.SpawnClone();
                }

                bool shadowSpawnProgress = ++boss.AICounter >= SHADOWSPAWNTIME;
                if (shadowSpawnProgress) {
                    boss.ResetCounters();
                    boss.DeactivateState<Phase1ShadowSpawn>();
                    boss.ActivateState<Phase1LaserAttack>();
                }
            }

            prepareClone();
        }

        void IAIState.OnStart(NPC npc, EternalHorror boss) {

        }

        void IAIState.OnEnd(NPC npc, EternalHorror boss) {

        }
    }

    private readonly struct Phase1DashAttack : IAIState {
        public static float DASHTIME => Helper.SecondsToFrames(0.75f);
        public static float LASERATTACKCOUNTNEEDED => 5;
        public static float DASHATTACKCOUNT => 5;

        public static SoundStyle DashSound => SoundID.Roar with { PitchVariance = 0.15f, MaxInstances = 0 };

        void IAIState.OnActiveUpdate(NPC npc, EternalHorror boss) {
            Player target = npc.GetTargetPlayer();
            Vector2 targetCenter = target.Center,
                    baseTargetCenter = targetCenter;
            targetCenter += targetCenter.DirectionTo(npc.Center) * 10f;

            float dashStrength = 40f;

            float dashProgress = boss.AICounter / DASHTIME;

            bool didAtLeastOneDash = boss.Phase1DashAttackCount > 0;

            float dashPreparationFactor = 0.25f;

            bool shouldResetState() {
                if (boss.Phase1DashAttackCount >= DASHATTACKCOUNT) {
                    boss.ActivateState<MoveToPlayer>();
                    boss.DeactivateState<Phase1DashAttack>();
                    boss.ActivateState<Phase1LaserAttack>();
                    boss.ResetCounters();

                    boss.SmoothFactor = 0.25f;
                    boss.Phase1DashAttackCount = 0;

                    boss._dashVelocity *= 0f;

                    return true;
                }

                return false;
            }
            void lookAtTarget() {
                if (!boss.HasActiveState<MoveToPlayer>()) {
                    boss.SmoothFactor = Helper.Approach(boss.SmoothFactor, dashProgress, 0.025f);
                }
                float angleToTarget = npc.AngleTo(baseTargetCenter) - MathHelper.PiOver2;
                npc.rotation = npc.rotation.AngleLerp(angleToTarget, ROTATIONLERP * boss.SmoothFactor);
            }
            void prepareDash() {
                boss.AICounter += 1f;
                if (!didAtLeastOneDash) {
                    boss.AICounter -= 0.25f;
                }
                bool shouldDash = boss.AICounter >= DASHTIME;
                if (shouldDash) {
                    if (shouldResetState()) {
                        return;
                    }

                    npc.ResetTrails();

                    SoundEngine.PlaySound(DashSound, npc.Center);

                    boss.AICounter = -DASHTIME;

                    boss.DeactivateState<MoveToPlayer>();

                    Vector2 dashDirection = npc.DirectionTo(targetCenter);
                    boss._dashVelocity = dashDirection * dashStrength;

                    boss.OnIterateActiveCloneData((ref CloneInfo cloneInfo) => {
                        Vector2 clonePosition = cloneInfo.VisualPosition;
                        dashDirection = clonePosition.DirectionTo(targetCenter);
                        cloneInfo.Velocity = dashDirection * dashStrength;
                    });

                    boss.Phase1DashAttackCount++;

                    boss.SmoothFactor = 0f;
                }
                else if (!didAtLeastOneDash) {
                    float smoothFactor = dashProgress;
                    dashProgress *= 1f - Utils.GetLerpValue(1f - dashPreparationFactor, 1f, smoothFactor, true);
                    npc.velocity -= npc.DirectionTo(targetCenter) * 1f * dashProgress;
                }
            }
            void slowDown() {
                float velocityDeceleration = 0.98f;
                boss._dashVelocity *= velocityDeceleration;
                boss.OnIterateActiveCloneData((ref CloneInfo cloneInfo) => cloneInfo.Velocity *= velocityDeceleration);
                boss._dashOpacity = Helper.Approach(boss._dashOpacity, boss._dashVelocity.Length() / dashStrength, 1f);
            }
            bool shouldSlowDownAfterDash() {
                bool preparingDash = boss.AICounter < 0f;
                if (preparingDash) {
                    return true;
                }

                return false;
            }
            void extraSlowDownAfterDash() {
                slowDown();

                if (didAtLeastOneDash) {
                    float smoothFactor = boss.SmoothFactor;
                    smoothFactor *= 1f - Utils.GetLerpValue(1f - dashPreparationFactor, 1f, smoothFactor, true);
                    npc.velocity += npc.DirectionTo(targetCenter) * 1f * smoothFactor;
                }
            }

            prepareDash();

            if (shouldSlowDownAfterDash()) {
                if (didAtLeastOneDash) {
                    float lerpValue = 0.25f;
                    npc.velocity = Vector2.Lerp(npc.velocity, boss._dashVelocity, lerpValue);
                }

                slowDown();
                return;
            }

            lookAtTarget();
            extraSlowDownAfterDash();
        }

        void IAIState.OnStart(NPC npc, EternalHorror boss) {

        }

        void IAIState.OnEnd(NPC npc, EternalHorror boss) {
        }
    }

    private Dictionary<Type, IAIState> _states = null;
    private HashSet<IAIState> _activeStates = null;

    public ref float Phase1LaserRotation => ref NPC.ai[2];

    public float Phase1LaserAttackProgress => Helper.Clamp01(AICounter / Phase1LaserAttack.LASERATTACKTIME);
    public float Phase1ShadowSpawnProgress => Helper.Clamp01(AICounter / Phase1ShadowSpawn.SHADOWSPAWNTIME);

    private partial void InitializeStates() {
        _states = [];
        _activeStates = [];
    }

    private void AddState<T>() where T : struct, IAIState => _states.TryAdd(typeof(T), new T());

    private void ActivateState<T>() where T : struct, IAIState {
        if (!Init) {
            return;
        }
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
        if (!Init) {
            return;
        }
        IAIState stateToDeactivate = _states[typeof(T)];
        if (!_activeStates.Contains(stateToDeactivate)) {
            return;
        }
        stateToDeactivate.OnEnd(npc: NPC, boss: Self);
        _activeStates.Remove(stateToDeactivate);
    }

    private bool HasActiveState<T>() where T : IAIState {
        if (!Init) {
            return false;
        }
        if (_states.TryGetValue(typeof(T), out IAIState state)) {
            return _activeStates.Contains(state);
        }
        return false;
    }
}
