using FluidHTN;
using FluidHTN.Operators;

namespace Rioters.Operators {
    public class FindNearestClusterOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if (ctx is NpcHtnContext c) {
                c.MoveToTarget = c.ClustersApproximator.GetClosestApproximatedClusterPosition(c.Position);

                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }


        public void Stop(IContext ctx) {

        }

    }
}