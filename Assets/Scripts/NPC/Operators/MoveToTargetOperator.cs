using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class MoveToTargetOperator : IOperator {

        private float _satisfactoryRadius;

        public MoveToTargetOperator(float satisfactoryRadius = 0f) { _satisfactoryRadius = satisfactoryRadius; }


        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                if ( c.NavAgent.isStopped )
                    return Move(c);

                return CheckForSuccess(c);
            }

            return TaskStatus.Failure;
        }


        private TaskStatus Move(NpcHtnContext c) {
            Debug.Log("Init move to cluster");
            if ( c.NavAgent.SetDestination(c.MoveToTarget) ) {
                c.NavAgent.isStopped = false;
                c.anim.SetBool("IsRunning", true);

                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }


        public void Stop(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                c.NavAgent.isStopped = true;
                c.anim.SetBool("IsRunning", false);
            }
        }


        private TaskStatus CheckForSuccess(NpcHtnContext c) {
            if ( !c.NavAgent.pathPending && c.NavAgent.remainingDistance <= c.NavAgent.radius + _satisfactoryRadius ) {
                c.NavAgent.isStopped = true;
                c.anim.SetBool("IsRunning", false);
                return TaskStatus.Success;
            }

            return TaskStatus.Continue;
        }

    }
}