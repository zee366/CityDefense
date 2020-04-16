using System;
using FluidHTN;
using FluidHTN.Conditions;

namespace Rioters.Conditions {
    public class HasWorldStateGreaterThanCondition : ICondition {

        public string       Name  { get; }
        public RiotersWorldState State { get; }
        public byte         Value { get; }


        public HasWorldStateGreaterThanCondition(RiotersWorldState state, byte value) {
            Name  = $"HasStateGreaterThan({state})";
            State = state;
            Value = value;
        }


        public bool IsValid(IContext ctx) {
            if ( ctx is RioterHTNContext c ) {
                var currentValue = c.GetState(State);
                var result       = currentValue > Value;
                if ( ctx.LogDecomposition )
                    ctx.Log(Name,
                            $"HasWorldStateGreaterThanCondition.IsValid({State}:{currentValue} > {Value} = {result})",
                            ctx.CurrentDecompositionDepth + 1,
                            this);

                return result;
            }

            throw new Exception("Unexpected context type!");
        }

    }
}