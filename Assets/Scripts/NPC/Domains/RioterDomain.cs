using FluidHTN;
using Rioters.Operators;
using UnityEngine;

namespace Rioters {
    [CreateAssetMenu(fileName = "RioterDomain", menuName = "AI/Domains/Rioter")]
    public class RioterDomain : AIDomainDefinition {

        public override Domain<NpcHtnContext> Create() {

            // Defining Regrouping domain
            var regroup = new NpcDomainBuilder("RegroupingDomain")
                .Sequence("Regrouping")
                    .Action("FindCluster").SetOperator(new FindNearestClusterOperator()).End()
                    .Action("GoToCluster").SetOperator(new MoveToTargetOperator(2f))
                        .IncrementState(NpcWorldState.StaminaLevel, 2, EffectType.PlanAndExecute)
                    .End()
                .End()
                .Build();

            return new NpcDomainBuilder("Rioter")
                // High priority first
                .Select("Receive Effect from police actions (Damage, Stun, wtv)")
                // TODO
                .End()
                .Select("Police close & enough stamina, flee!")
                    .Sequence("To flee")
                        .HasState(NpcWorldState.PoliceInRange)
                        // .Condition("Has police in range", (ctx) => ctx.HasState(NpcWorldState.PoliceInRange))
                        .HasStateGreaterThan(NpcWorldState.StaminaLevel, 2)  // This is conceptual only for now. Would actively flee only if has enough energy.
                        .PrimitiveTask<FindPolice>("Find closest police").End()
                        .Flee(NpcType.Police) // Self contained task
                            .DecrementState(NpcWorldState.StaminaLevel, EffectType.PlanAndExecute)
                        .End()
                    .End()
                .End()
                .Select("Towards closest destructible, or regroup")
                    .Sequence("To destructible")
                        .HasStateGreaterThan(NpcWorldState.StaminaLevel, 1) // Need at least a basic level of stamina
                        .Condition("Has potential targets", (ctx) => ctx.HasState(NpcWorldState.HasDestructiblesInRange))
                        .PrimitiveTask<FindDestructible>("Find closest target").End()
                        .MoveToDestructible()    // Self contained Task
                        .PrimitiveTask<DamageDestructible>("Deal damage to target")
                            // .HasState(NpcWorldState.TargetInAttackRange)
                            .DecrementState(NpcWorldState.StaminaLevel, EffectType.PlanAndExecute)
                        .End()
                    .End()
                    .Splice(regroup)    // Regrouping subdomain defined above
                .End()
                .Build();
        }

    }
}