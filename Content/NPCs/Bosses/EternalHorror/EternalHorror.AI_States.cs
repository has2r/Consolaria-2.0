using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    private interface IAIState {
        public void OnActiveUpdate(NPC npc, EternalHorror boss);
    }

    private readonly struct MoveToPlayer : IAIState {
        void IAIState.OnActiveUpdate(NPC npc, EternalHorror boss) {
            boss.TargetPlayer();

            Player target = npc.GetTargetPlayer();
            Vector2 targetCenter = target.Center;
            
            const int minDistanceToTargetInPixels = 300;

            void lookAtTarget() {
                float angleToTarget = npc.AngleTo(targetCenter) - MathHelper.PiOver2;
                npc.rotation = angleToTarget;
            }
            void makeTargetPositionABitHigher() {
                targetCenter.Y -= 100f;
            }
            void moveToTarget() {
                const float speed = 15f,
                            inertia = 20f;
                const float deceleration = 0.99f;
                npc.MoveToWithDeceleration(targetCenter, speed, inertia, minDistanceToTargetInPixels, deceleration);

                Vector2 targetPosition = Vector2.Zero.MoveTowards(targetCenter - npc.Center, 4f);
                npc.velocity = npc.velocity.MoveTowards(targetPosition, 2f / 15f);
            }
            void moveFromTargetIfClose() {
                if (npc.Distance(targetCenter) < minDistanceToTargetInPixels / 2f) {
                    npc.velocity += npc.DirectionFrom(targetCenter) * 0.25f;
                }
            }
            void moveUpwardsIfClose() {
                if (npc.Center.Y > targetCenter.Y) {
                    npc.velocity -= Vector2.UnitY * 0.25f;
                }
            }

            lookAtTarget();
            makeTargetPositionABitHigher();
            moveToTarget();
            moveFromTargetIfClose();
            moveUpwardsIfClose();
        }
    }

    private readonly struct Phase1LaserAttack : IAIState {
        public static float LASERATTACKTIME => Helper.SecondsToFrames(1);

        void IAIState.OnActiveUpdate(NPC npc, EternalHorror boss) {
            boss._glowOpacity = Helper.Approach(boss._glowOpacity, boss.Phase1LaserAttackProgress, 0.1f);

            if (++boss.AICounter <= LASERATTACKTIME) {
                return;
            }

            boss.AICounter = -LASERATTACKTIME / 2f;
        }
    }

    private Dictionary<Type, IAIState> _states = null!;
    private HashSet<IAIState> _activeStates = null!;

    public float Phase1LaserAttackProgress => Helper.Clamp01(AICounter / Phase1LaserAttack.LASERATTACKTIME);

    private void InitializeStates() {
        _states = [];
        _activeStates = [];
    }

    private void AddState<T>() where T : struct, IAIState => _states.TryAdd(typeof(T), new T());

    private void ActivateState<T>() where T : struct, IAIState {
        AddState<T>();
        IAIState stateToActivate = _states[typeof(T)];
        _activeStates.Add(stateToActivate);
    }

    private void DeactivateState<T>() where T : IAIState {
        IAIState stateToDeactivate = _states[typeof(T)];
        _activeStates.Remove(stateToDeactivate);
    }

    private bool HasActiveState<T>() where T : IAIState {
        if (_states.TryGetValue(typeof(T), out IAIState state)) {
            return _activeStates.Contains(state);
        }
        return false;
    }
}
