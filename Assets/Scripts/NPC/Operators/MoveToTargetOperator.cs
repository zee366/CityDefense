using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class MoveToTargetOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {

                if(c.NavAgent.isStopped)
                    return Move(c);

                return CheckForSuccess(c);
            }

            return TaskStatus.Failure;
        }


        private TaskStatus Move(NpcHtnContext c) {
            Debug.Log("Init move to cluster");
            if ( c.NavAgent.SetDestination(c.MoveToTarget) ) {
                c.NavAgent.isStopped = false;
                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }


        public void Stop(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                c.NavAgent.isStopped = true;
            }
        }


        private TaskStatus CheckForSuccess(NpcHtnContext c) {
            if ( !c.NavAgent.pathPending && c.NavAgent.remainingDistance <= c.NavAgent.radius ) {
                c.NavAgent.isStopped = true;
                return TaskStatus.Success;
            }

            return TaskStatus.Continue;
        }

    }
}