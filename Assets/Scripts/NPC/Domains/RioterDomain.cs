using FluidHTN;
using UnityEngine;

namespace Rioters {
    [CreateAssetMenu(fileName = "RioterDomain", menuName = "AI/Domains/Rioter")]
    public class RioterDomain : AIDomainDefinition {

        public override Domain<NpcHtnContext> Create() {
            return new NpcDomainBuilder("Rioter")
                .Select("Receive Damage")    // TODO
                .End()
                .Select("Police close & enough stamina, flee!")
                    .HasState(NpcWorldState.PoliceInRange)
                    .Action("Flee from police squad") // TODO
                    .SetState(NpcWorldState.PoliceInRange, false, EffectType.PlanAndExecute)
                    .End()
                .End()
                .Select("Towards closest destructible, or regroup")
                    .Sequence("To destructible")
                        .Condition("Has potential targets", (ctx) => ctx.HasState(NpcWorldState.HasDestructiblesInRange))
                        .PrimitiveTask<FindDestructible>("Find closest target")
                        .End()
                        .MoveToDestructible()    // Self contained Task
                        .PrimitiveTask<DamageDestructible>("Deal damage to target")
                            .Condition("At target", (ctx) => ctx.HasState(NpcWorldState.TargetInAttackRange))
                            .Effect("Dealt damage", EffectType.PlanAndExecute,
                                    (ctx, type) => {
                                        ctx.SetState(NpcWorldState.TargetInAttackRange, false, type);
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