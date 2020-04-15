using FluidHTN;
using UnityEngine;

namespace Rioters {
    [CreateAssetMenu(fileName = "RioterDomain", menuName = "AI/Domains/Rioter")]
    public class RioterDomain : AIDomainDefinition {

        public override Domain<NpcHtnContext> Create() {
            return new NpcDomainBuilder("Rioter")
                // High priority first
                .Select("Receive Effect from police actions (Damage, Stun, wtv)")    // TODO
                .End()
                .Select("Police close & enough stamina, flee!")
                    .HasState(NpcWorldState.PoliceInRange)
                    .HasStateGreaterThan(NpcWorldState.StaminaLevel, 2)  // This is conceptual only for now. Would actively flee only if has enough energy.
                    .PrimitiveTask<FindPolice>("Find closest police").End()
                    .Flee(NpcType.Police)
                        .SetState(NpcWorldState.PoliceInRange, false, EffectType.PlanAndExecute)
                        .DecrementState(NpcWorldState.StaminaLevel, EffectType.PlanAndExecute)
                    .End()
                .End()
                .Select("Towards closest destructible, or regroup")
                    .Sequence("To destructible")
                        .HasStateGreaterThan(NpcWorldState.StaminaLevel, 1) // Need at least a basic level of stamina
                        .Condition("Has potential targets", (ctx) => ctx.HasState(NpcWorldState.HasDestructiblesInRange))
                        .PrimitiveTask<FindDestructible>("Find closest target").End()
                        .MoveToDestructible()    // Self contained Task
                        .PrimitiveTask<DamageDestructible>("Deal damage to target")
                            .Condition("At target", (ctx) => ctx.HasState(NpcWorldState.TargetInAttackRange))
                            .Effect("Dealt damage", EffectType.PlanAndExecute,
                                    (ctx, type) => {
                                        ctx.SetState(NpcWorldState.TargetInAttackRange, false, type);
                                    })
                        .End()
                        .DecrementState(NpcWorldState.StaminaLevel, EffectType.PlanAndExecute)
                    .End()
                    .Sequence("Regroup")
                        // TODO
                        .Action("Regroup action").Do((ctx) => { Debug.Log("Would regroup"); return TaskStatus.Success; })
                        .End()
                        .IncrementState(NpcWorldState.StaminaLevel, EffectType.PlanAndExecute)
                    .End()
                .End()
                .Build();
        }

    }
}