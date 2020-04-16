using System;
using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class FleeOperator : IOperator {

        private NpcType _fleeType;

        private float   _fleeingTargetUpdateInterval;
        private float   _timeSinceLastFleePosUpdate;
        private Vector3 _targetPos;


        public FleeOperator(NpcType typeToFlee, float fleeingTargetUpdateInterval) {
            _fleeType                    = typeToFlee;
            _fleeingTargetUpdateInterval = fleeingTargetUpdateInterval;
            _timeSinceLastFleePosUpdate  = fleeingTargetUpdateInterval + 1; // We want to always start by computing the target position
        }


        public TaskStatus Update(IContext ctx) {
            Debug.Log("in fleeing op1");
            if ( ctx is NpcHtnContext c ) {
                Debug.Log("in fleeing op2");
                // Update new flee position at interval
                _timeSinceLastFleePosUpdate += Time.deltaTime;
                if ( _timeSinceLastFleePosUpdate > _fleeingTargetUpdateInterval ) {
                    _timeSinceLastFleePosUpdate = 0;

                    // Compute new flee position
                    try {
                        _targetPos = GetPosFromType(c, _fleeType);
                        Debug.Log("fleeing");
                        return Move(c);
                    }
                    catch ( Exception e ) {
                        return TaskStatus.Failure;
                    }
                } else {
                    // Continue
                    return CheckForSuccess(c);
                }
            }

            return TaskStatus.Failure;
        }


        private Vector3 GetPosFromType(NpcHtnContext ctx, NpcType fleeType) {
            Vector3 posToFleeTo;
            switch ( fleeType ) {
                case NpcType.Police:
                    posToFleeTo = ctx.Position - (ctx.ClosestPolice.transform.position - ctx.Position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fleeType), fleeType, null);
            }

            return posToFleeTo;
        }


        public void Stop(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                c.NavAgent.isStopped = true;
            }
        }


        private TaskStatus Move(NpcHtnContext c) {
            if ( c.NavAgent.SetDestination(_targetPos) ) {
                c.NavAgent.isStopped = false;
                return TaskStatus.Continue;
            }

            return TaskStatus.Failure;
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