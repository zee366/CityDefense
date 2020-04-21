using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class WanderOperator : IOperator {

        private float _wanderRadius;
        private int   _nbWanderPositionToReach;
        private int   _totalWanderStepCount;
        private float _timeBetweenWanderPositions;
        private float _idleTimer;

        private bool IsIdle { get; set; }


        public WanderOperator(float wanderRadius = 10f, int nbWanderStep = 2, float wanderIdleStepTime = 1f) {
            _wanderRadius               = wanderRadius;
            _nbWanderPositionToReach    = nbWanderStep;
            _timeBetweenWanderPositions = wanderIdleStepTime;
        }


        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                if ( IsIdle ) {
                    _idleTimer += Time.deltaTime;
                    if ( _idleTimer >= _timeBetweenWanderPositions )
                        IsIdle = false;
                } else {
                    if ( c.NavAgent.isStopped )
                        return StartWander(c);

                    if ( CheckForWanderTargetReached(c) ) {
                        // Start idling a little
                        _totalWanderStepCount++;
                        IsIdle     = true;
                        _idleTimer = 0;
                    }
                }

                if ( _totalWanderStepCount >= _nbWanderPositionToReach )
                    return TaskStatus.Success;

                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
        }


        private TaskStatus StartWander(NpcHtnContext c) {
            // Random position around self
            Vector3 randPos = c.Position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * _wanderRadius;

            if ( c.NavAgent.SetDestination(randPos) ) {
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
                _totalWanderStepCount = 0;
                _idleTimer            = 0;
            }
        }


        private bool CheckForWanderTargetReached(NpcHtnContext c) {
            if ( !c.NavAgent.pathPending && c.NavAgent.remainingDistance <= c.NavAgent.radius ) {
                c.NavAgent.isStopped = true;
                c.anim.SetBool("IsRunning", false);
                return true;
            }

            return false;
        }

    }
}