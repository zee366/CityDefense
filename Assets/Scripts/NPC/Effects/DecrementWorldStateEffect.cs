using System;
using FluidHTN;

namespace Rioters.Effects {
    public class DecrementWorldStateEffect : IEffect {

        public string       Name  { get; }
        public EffectType   Type  { get; }
        public NpcWorldState State { get; }
        public byte         Value { get; }


        public DecrementWorldStateEffect(NpcWorldState state, EffectType type) {
            Name  = $"DecrementState({state})";
            Type  = type;
            State = state;
            Value = 1;
        }


        public DecrementWorldStateEffect(NpcWorldState state, byte value, EffectType type) {
            Name  = $"DecrementState({state})";
            Type  = type;
            State = state;
            Value = value;
        }


        public void Apply(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                var currentValue = c.GetState(State);
                c.SetState(State, (byte) (currentValue - Value), Type);
                if ( ctx.LogDecomposition )
                    ctx.Log(Name, $"DecrementWorldStateEffect.Apply({State}:{currentValue}+{Value}:{Type})", ctx.CurrentDecompositionDepth + 1, this);

                return;
            }

            throw new Exception("Unexpected context type!");
        }

    }
}