using FluidHTN;
using UnityEngine;

namespace Rioters {
    [CreateAssetMenu(fileName = "RioterDomain", menuName = "AI/Domains/Rioter")]
    public class RioterDomain : AIDomainDefinition {

        public override Domain<RioterHTNContext> Create() {
            return new RiotersDomainBuilder("Rioter")
                .Sequence("Find destructible target")
                    .PrimitiveTask<FindDestructible>("Find target")
                        .Condition("Has no target", (ctx) => !ctx.HasState(RiotersWorldState.FoundTargetDestructible))
                        .Effect("Set target here", EffectType.PlanAndExecute,
                                   (ctx, type) => { ctx.SetState(RiotersWorldState.FoundTargetDestructible, true, type); })
                    .End()
                    .MoveToDestructible()
                    .End()
                    .PrimitiveTask<DamageDestructible>("Deal damage to target")
                        .Condition("At target", (ctx) => ctx.HasState(RiotersWorldState.TargetInRange))
                        .Effect("Dealt damage", EffectType.PlanAndExecute, (ctx, type) => { })
                    .End()
                .End()
                .Build();
        }

    }
}