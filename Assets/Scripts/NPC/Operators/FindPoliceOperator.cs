using System;
using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class FindPoliceOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {
                Transform _closest = null;
                float _closestDist = float.PositiveInfinity;

                foreach ( Transform police in c.policesInRange ) {
                    if ( _closest == null ) {
                        _closest = police;
                        continue;
                    }

                    Vector3 closestPoint = police.GetComponent<Collider>().ClosestPoint(c.Position);
                    float dist = Vector3.Distance(closestPoint, c.Position);
                    if ( dist < _closestDist ) {
                        _closestDist = dist;
                        _closest = police;
                    }
                }

                if (_closest != null) {
                    c.ClosestPolice = _closest;
                    return TaskStatus.Success;
                }
                return TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }


        public void Stop(IContext ctx) { }

    }
}