using System;
using FluidHTN;
using FluidHTN.Conditions;

namespace Rioters.Conditions {
    public class HasWorldStateCondition : ICondition {

        public string            Name  { get; }
        public RiotersWorldState State { get; }
        public byte              Value { get; }


        public HasWorldStateCondition(RiotersWorldState state) {
            Name  = $"HasState({state})";
            State = state;
            Value = 1;
        }


        public HasWorldStateCondition(RiotersWorldState state, byte value) {
            Name  = $"HasState({state})";
            State = state;
            Value = value;
        }


        public bool IsValid(IContext ctx) {
            if ( ctx is RioterHTNContext c ) {
                var result = c.HasState(State, Value);
                if ( ctx.LogDecomposition ) ctx.Log(Name, $"HasWorldStateCondition.IsValid({State}:{Value}:{result})", ctx.CurrentDecompositionDepth + 1, this);
                return result;
            }

            throw new Exception("Unexpected context type!");
        }

    }
}