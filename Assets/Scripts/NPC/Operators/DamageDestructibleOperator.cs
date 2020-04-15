using FluidHTN;
using FluidHTN.Operators;

namespace Rioters.Operators {
    public class DamageDestructibleOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {

                // TODO: Get attack damage from context
                c.CurrentTarget.TakeDamage(0.3f);

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