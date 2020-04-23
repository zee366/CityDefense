using FluidHTN.PrimitiveTasks;
using Rioters.Operators;

namespace Rioters {
    public class FindDestructible : PrimitiveTask {

        public FindDestructible() {
            SetOperator(new FindDestructibleOperator());
        }

    }
}