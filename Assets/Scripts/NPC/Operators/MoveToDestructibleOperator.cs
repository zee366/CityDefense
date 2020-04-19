using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class MoveToDestructibleOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c && c.CurrentTarget != null ) {
                if ( c.NavAgent.isStopped ) {
                    return StartMove(c);
                } else {
                    return UpdateMove(c);
                }
            }

            return TaskStatus.Failure;
        }


        private TaskStatus StartMove(NpcHtnContext c) {
            if ( c.CurrentTarget == null )
                return TaskStatus.Failure;

            Vector3 closestTargetBound = c.CurrentTarget.GetComponent<Collider>().ClosestPointOnBounds(c.Position);

            if(c.Verbose)
                Debug.Log("Would ask a new path");

            if ( c.NavAgent.SetDestination(closestTargetBound) ) {
                c.NavAgent.isStopped = false;
                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }


        private TaskStatus UpdateMove(NpcHtnContext c) {
            if(c.Verbose)
                Debug.Log("MoveToDestr pendingState : "+ c.NavAgent.pathPending);

            if(!c.NavAgent.pathPending)
                c.anim.SetBool("IsRunning", true);

            if ( !c.NavAgent.pathPending && c.NavAgent.remainingDistance <= c.NavAgent.radius ) {
                c.NavAgent.isStopped = true;
                c.anim.SetBool("IsRunning",false);
                return TaskStatus.Success;
            }

            return TaskStatus.Continue;
        }

        public void Stop(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                c.NavAgent.isStopped = true;
                c.anim.SetBool("IsRunning", false);
            }
        }

    }
}