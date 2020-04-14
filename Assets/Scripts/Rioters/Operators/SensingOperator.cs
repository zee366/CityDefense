using System.CodeDom;
using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class SensingOperator : IOperator {

        private float _timeSinceLastSensing;

        public TaskStatus Update(IContext ctx) {
            // TODO: Add sensing interval
            if ( ctx is RioterHTNContext c ) {
                // Sense
                // Collider[] colliders = Physics.OverlapSphere(c.Position, c.SensingRange);

                // Check for
                // c.destructiblesInRange.Add();

                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

        public void Stop(IContext ctx) { }

    }
}