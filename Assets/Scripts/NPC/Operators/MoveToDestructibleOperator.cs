using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;
using UnityEngine.AI;

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

            // To get random position AROUND the target object
            Bounds bounds = c.CurrentTarget.GetComponent<Collider>().bounds;
            Vector3 randomizePosition = bounds.center + new Vector3(bounds.extents.x*Random.Range(-1f, 1f), 0, bounds.extents.z*Random.Range(-1f, 1f));

            // MaxDistance = 1000 simply because we don't want to be really limited by scale of objects
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomizePosition, out hit, 1000f, NavMesh.AllAreas)){
                if ( c.NavAgent.SetDestination(hit.position) ) {
                    c.NavAgent.isStopped = false;
                    return TaskStatus.Continue;
                }

                return TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }


        private TaskStatus UpdateMove(NpcHtnContext c) {
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