using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class DamageDestructibleOperator : IOperator {

        private float _attackTime = 0f;

        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c && c.CurrentTarget != null ) {
                _attackTime += Time.deltaTime;

                // Setting animation
                c.anim.SetBool("IsAttacking",true);
                c.CurrentTarget.TakeDamage(Time.deltaTime * c.DPS);

                // Check if totally destroyed
                if ( c.CurrentTarget.IsDead || _attackTime >= c.MaxAttackActionLength ) {
                    c.anim.SetBool("IsAttacking", false);
                    _attackTime = 0;    // Need to reset internal state since it's kept in memory for next planning
                    return TaskStatus.Success;
                }

                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }


        public void Stop(IContext ctx) {
            if(ctx is NpcHtnContext c)
                c.anim.SetBool("IsAttacking", false);
        }


    }
}