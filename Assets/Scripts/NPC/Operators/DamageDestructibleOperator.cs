using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class DamageDestructibleOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {

                c.CurrentTarget.TakeDamage(Time.deltaTime * c.DPS);

                // TODO: Play animation here

                // Check if totally destroyed
                if ( c.CurrentTarget.IsDead ) {
                    return TaskStatus.Success;
                }

                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }

        public void Stop(IContext ctx) { }

    }
}