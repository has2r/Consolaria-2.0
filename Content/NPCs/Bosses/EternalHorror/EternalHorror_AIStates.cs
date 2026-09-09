using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Consolaria.Content.NPCs.Bosses.EternalHorror;

sealed partial class EternalHorror : ModNPC {
    private interface IAIState {
        public void OnActiveUpdate(EternalHorror boss);
    }

    public readonly struct IdleState : IAIState {
        void IAIState.OnActiveUpdate(EternalHorror boss) {
            
        }
    }

    private Dictionary<Type, IAIState> _states = null!;
    private IAIState _activeState;

    private void AddState<T>() where T : struct, IAIState => _states.TryAdd(typeof(T), new T());

    private void ChangeState<T>() where T : struct, IAIState {
        IAIState newState = _states[typeof(T)];
        if (_activeState == newState) {
            return;
        }

        _activeState = newState;
    }
}
