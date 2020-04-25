using System;
using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators {
    public class FindDestructibleOperator : IOperator {

        public TaskStatus Update(IContext ctx) {
            if ( ctx is NpcHtnContext c ) {

                Destructible _closest = null;
                float _closestDist = float.PositiveInfinity;

                foreach ( Destructible destructible in c.destructiblesInRange ) {
                    if ( _closest == null ) {
                        _closest = destructible;
                        continue;
                    }

                    Vector3 closestPoint = destructible.GetComponent<Collider>().ClosestPointOnBounds(c.Position);
                    float dist = Vector3.Distance(closestPoint, c.Position);
                    if ( dist < _closestDist ) {
                        _closestDist = dist;
                        _closest = destructible;
                    }
                }

                c.CurrentTarget = _closest;

                if (_closest != null)
                    return TaskStatus.Success;

                return TaskStatus.Failure;
            }

            return TaskStatus.Failure;
        }


        public void Stop(IContext ctx) { }

    }
}