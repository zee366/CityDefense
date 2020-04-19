using FluidHTN;
using UnityEngine;

namespace Rioters {
    [CreateAssetMenu(fileName = "RioterDomain", menuName = "AI/Domains/Rioter")]
    public class RioterDomain : AIDomainDefinition {

        public override Domain<NpcHtnContext> Create() {
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
                            .SetState(NpcWorldState.TargetInAttackRange, true, EffectType.PlanAndExecute)
                        .End()
                        .DecrementState(NpcWorldState.StaminaLevel, EffectType.PlanAndExecute)
                    .End()
                    .Sequence("Regroup")
                        // Twice for cleaner regrouping from approximation and looks better.
                        .Regroup()
                        .Regroup()
                        // .IncrementState(NpcWorldState.StaminaLevel, 5, EffectType.PlanAndExecute)
                    .End()
                .End()
                .Build();
        }

    }
}