using FluidHTN.PrimitiveTasks;
using Rioters.Operators;

namespace Rioters {
    public class FindPolice : PrimitiveTask {

        public FindPolice() {
            SetOperator(new FindPoliceOperator());
        }

    }
}