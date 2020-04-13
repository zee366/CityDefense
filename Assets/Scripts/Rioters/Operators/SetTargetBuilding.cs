using System;
using FluidHTN;
using UnityEngine;

namespace Rioters.Operators {
    public class SetTargetBuilding : IEffect {

        private Transform _transform;

        public SetTargetBuilding(Transform buildingTransform) { _transform = buildingTransform; }

        public string Name { get; } = "Set target building";
        public EffectType Type { get; } = EffectType.PlanAndExecute;


        public void Apply(IContext ctx) {
            if (ctx is RioterHTNContext c)
                c.SetState(RiotersWorldState.HasTargetBuilding, true, Type);
            else
                throw new Exception("Unexpected context type!");
        }

    }
}