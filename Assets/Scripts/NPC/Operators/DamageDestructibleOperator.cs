using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class DamageDestructibleOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                //Setting animation
                c.anim.SetBool("IsAttacking",true);
                c.CurrentTarget.TakeDamage(Time.deltaTime * c.DPS);

                // Check if totally destroyed
                if ( c.CurrentTarget.IsDead ) {
                    c.anim.SetBool("IsAttacking",false);
                    return TaskStatus.Success;
                }

                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }

        public void Stop(IContext ctx) { }

    }
}