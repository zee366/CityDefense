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


        public void Stop(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                c.NavAgent.isStopped = true;
                c.anim.SetBool("IsRunning", false);
            }
        }


        private TaskStatus StartMove(NpcHtnContext c) {
            if ( c.CurrentTarget == null )
                return TaskStatus.Failure;

            Vector3 closestTargetBound = c.CurrentTarget.GetComponent<Collider>().ClosestPointOnBounds(c.Position);
            if ( c.NavAgent.SetDestination(closestTargetBound) ) {
                c.anim.SetBool("IsRunning",true);
                c.NavAgent.isStopped = false;
                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }


        private TaskStatus UpdateMove(NpcHtnContext c) {
            Vector3 closestTargetBound = c.CurrentTarget.GetComponent<Collider>().ClosestPointOnBounds(c.Position);
            c.NavAgent.SetDestination(closestTargetBound);
            if ( !c.NavAgent.pathPending && c.NavAgent.remainingDistance <= c.NavAgent.radius ) {
                c.NavAgent.isStopped = true;
                c.anim.SetBool("IsRunning",false);
                return TaskStatus.Success;
            }

            return TaskStatus.Continue;
        }

    }
}