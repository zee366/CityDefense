using System;
using FluidHTN;

namespace Rioters.Effects {
    public class SetWorldStateEffect : IEffect {

        public string       Name  { get; }
        public EffectType   Type  { get; }
        public NpcWorldState State { get; }
        public byte         Value { get; }


        public SetWorldStateEffect(NpcWorldState state, EffectType type) {
            Name  = $"SetState({state})";
            Type  = type;
            State = state;
            Value = 1;
        }


        public SetWorldStateEffect(NpcWorldState state, bool value, EffectType type) {
            Name  = $"SetState({state})";
            Type  = type;
            State = state;
            Value = (byte) (value ? 1 : 0);
        }


        public SetWorldStateEffect(NpcWorldState state, byte value, EffectType type) {
            Name  = $"SetState({state})";
            Type  = type;
            State = state;
            Value = value;
        }


        public void Apply(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                if ( ctx.LogDecomposition ) ctx.Log(Name, $"SetWorldStateEffect.Apply({State}:{Value}:{Type})", ctx.CurrentDecompositionDepth + 1, this);
                c.SetState(State, Value, Type);
                return;
            }

            throw new Exception("Unexpected context type!");
        }

    }
}