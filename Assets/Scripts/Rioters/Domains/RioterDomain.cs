using FluidHTN;
using UnityEngine;

namespace Rioters {
    [CreateAssetMenu(fileName = "RioterDomain", menuName = "AI/Domains/Rioter")]
    public class RioterDomain : AIDomainDefinition {

        public override Domain<RioterHTNContext> Create() {
            return new DomainBuilder<RioterHTNContext>("MonTest")
                   .Select("first")
                   .Condition("Had no target", (ctx) => ctx.HasState(RiotersWorldState.HasTargetBuilding))
                   .Action("Find New building target")
                   .Do((ctx) => {
                       Debug.Log("Finding new building");
                       return TaskStatus.Success;
                   })
                   .Effect("Set target here",
                           EffectType.PlanAndExecute,
                           (ctx, type) => { ctx.SetState(RiotersWorldState.HasTargetBuilding, true, type); })
                   .End()
                   .End()
                   .Build();
        }

    }
}