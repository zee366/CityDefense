using FluidHTN;
using FluidHTN.Operators;

namespace Rioters.Operators {
    public class FindNearestClusterOperator : IOperator {

        private int _clusterKnnCount = 0;

        public FindNearestClusterOperator() { }

        public FindNearestClusterOperator(int knnSearchCount) { _clusterKnnCount = knnSearchCount; }


        public TaskStatus Update(IContext ctx) {
            if (ctx is NpcHtnContext c) {
                if(_clusterKnnCount==0)
                    c.MoveToTarget = c.ClustersApproximator.GetClosestApproximatedClusterPosition(c.Position);
                else {
                    c.MoveToTarget = c.ClustersApproximator.GetClosestApproximatedClusterPosition(c.Position, _clusterKnnCount);
                }

                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }


        public void Stop(IContext ctx) {

        }

    }
}