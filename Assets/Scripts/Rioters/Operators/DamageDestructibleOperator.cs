using FluidHTN;
using FluidHTN.Operators;

namespace Rioters.Operators {
    public class DamageDestructibleOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is RioterHTNContext c ) {

                // TODO: Get attack damage from context
                c.CurrentTarget.TakeDamage(1f);

                // TODO: Play animation here

                // Check if totally destroyed
                if ( c.CurrentTarget.health <= 0 ) {
                    return TaskStatus.Success;
                }

                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }

        public void Stop(IContext ctx) { }

    }
}