using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class MoveToDestructibleOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is RioterHTNContext c ) {
                if ( c.NavAgent.isStopped ) {
                    return StartMove(c);
                } else {
                    return UpdateMove(c);
                }
            }

            return TaskStatus.Failure;
        }


        public void Stop(IContext ctx) {
            if ( ctx is RioterHTNContext c ) {
                c.NavAgent.isStopped = true;
            }
        }


        private TaskStatus StartMove(RioterHTNContext c) {
            c.NavAgent.isStopped = false;
            if ( c.NavAgent.SetDestination(c.CurrentTarget.transform.position) ) {
                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }


        private TaskStatus UpdateMove(RioterHTNContext c) {
            if ( c.NavAgent.remainingDistance <= c.NavAgent.stoppingDistance ) {
                c.NavAgent.isStopped = true;
                return TaskStatus.Success;
            }

            return TaskStatus.Continue;
        }

    }
}