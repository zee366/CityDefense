using FluidHTN;
using FluidHTN.Operators;

namespace Rioters.Operators {
    public class DamageDestructibleOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is RioterHTNContext c ) {
                //Setting animation
                c.anim.SetBool("IsAttacking",true);
                // TODO: Get attack damage from context
                c.CurrentTarget.TakeDamage(0.3f);

                // TODO: Play animation here

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