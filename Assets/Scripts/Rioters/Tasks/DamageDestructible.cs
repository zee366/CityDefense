using FluidHTN.PrimitiveTasks;
using Rioters.Operators;

namespace Rioters {
    public class DamageDestructible : PrimitiveTask {

        public DamageDestructible() {
            SetOperator(new DamageDestructibleOperator());
        }

    }
}