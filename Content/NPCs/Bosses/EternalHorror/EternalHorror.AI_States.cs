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

            void smoothEverything() {
                boss.SmoothFactor = Helper.Approach(boss.SmoothFactor, 1f, 0.025f);
            }
            void lookAtTarget() {
                float angleToTarget = npc.AngleTo(baseTargetCenter) - MathHelper.PiOver2;
                npc.rotation = npc.rotation.AngleLerp(angleToTarget, ROTATIONLERP * boss.SmoothFactor);
            }
            void makeTargetPositionABitHigher() {
                if (boss.HasActiveState<Phase1DashAttack>()) {
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
                if (boss.HasActiveState<Phase1DashAttack>()) {
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
                if (boss.HasActiveState<Phase1DashAttack>()) {
                    return;
                }
                if (npc.Center.Y > targetCenter.Y) {
                    npc.velocity -= Vector2.UnitY * 0.5f * boss.SmoothFactor;
                }
            }
            void slowDownWhenCloseToTarget() {
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
        public static float DASHTIME => Helper.SecondsToFrames(1);
        public static float LASERATTACKCOUNTNEEDED => 5;
        public static float DASHATTACKCOUNT => 3;

        void IAIState.OnActiveUpdate(NPC npc, EternalHorror boss) {
            bool shouldDash = ++boss.AICounter >= DASHTIME;

            Player target = npc.GetTargetPlayer();
            Vector2 targetCenter = target.Center;

            void smoothEverything() {
                boss.SmoothFactor = Helper.Approach(boss.SmoothFactor, 1f, 0.025f);
            }
            void lookAtTarget() {
                float angleToTarget = npc.AngleTo(targetCenter) - MathHelper.PiOver2;
                npc.rotation = npc.rotation.AngleLerp(angleToTarget, ROTATIONLERP * boss.SmoothFactor);
            }

            if (shouldDash) {
                boss.AICounter = -DASHTIME / 2f;

                boss.DeactivateState<MoveToPlayer>();

                float dashStrength = 30f;
                Vector2 dashDirection = npc.DirectionTo(targetCenter);
                npc.velocity = dashDirection * dashStrength;

                boss.Phase1DashAttackCount++;

                boss.SmoothFactor = 0f;
            }

            bool preparingDash = boss.AICounter < 0f;
            if (preparingDash) {
                npc.velocity *= 0.98f;

                //float dashRotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
                //npc.rotation = npc.rotation.AngleLerp(dashRotation, ROTATIONLERP);

                return;
            }

            smoothEverything();

            lookAtTarget();

            npc.velocity *= 0.98f;

            if (boss.Phase1DashAttackCount >= DASHATTACKCOUNT) {
                boss.DeactivateState<Phase1DashAttack>();
                boss.ActivateState<Phase1LaserAttack>();
                boss.ResetCounters();

                boss.Phase1DashAttackCount = 0;
            }

            boss.ActivateState<MoveToPlayer>();
        }

        void IAIState.OnStart(NPC npc, EternalHorror boss) {

        }

        void IAIState.OnEnd(NPC npc, EternalHorror boss) {
        }
    }

    private Dictionary<Type, IAIState> _states = null!;
    private HashSet<IAIState> _activeStates = null!;

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
