using FluidHTN;
using Rioters.Operators;
using UnityEngine;

namespace Rioters {
    [CreateAssetMenu(fileName = "RioterDomain", menuName = "AI/Domains/Rioter")]
    public class RioterDomain : AIDomainDefinition {

        public override Domain<RioterHTNContext> Create() {
            return new RiotersDomainBuilder("Rioter")
                .Select("Towards closest destructible, or regroup")
                    .Sequence("To destructible")
                        .Condition("Has potential targets", (ctx) => ctx.HasState(RiotersWorldState.HasDestructiblesInRange))
                        .PrimitiveTask<FindDestructible>("Find closest target")
                        .End()
                        .MoveToDestructible()    // Self contained Task
                        .PrimitiveTask<DamageDestructible>("Deal damage to target")
                            .Condition("At target", (ctx) => ctx.HasState(RiotersWorldState.TargetInAttackRange))
                            .Effect("Dealt damage", EffectType.PlanAndExecute,
                                    (ctx, type) => {
                                        ctx.SetState(RiotersWorldState.TargetInAttackRange, false, type);
                                    })
                        .End()
                    .End()
                    .Sequence("Regroup")
                        // TODO
                        .Action("Regroup action").Do((ctx) => { Debug.Log("Would regroup"); return TaskStatus.Success; })
                        .End()
                    .End()
                .End()
                .Build();
        }

    }
}