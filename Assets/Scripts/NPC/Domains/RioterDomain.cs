using FluidHTN;
using Rioters.Operators;
using UnityEngine;

namespace Rioters {
    [CreateAssetMenu(fileName = "RioterDomain", menuName = "AI/Domains/Rioter")]
    public class RioterDomain : AIDomainDefinition {

        public override Domain<NpcHtnContext> Create() {

            // TODO: Define receiving effects/damage/etc here
            var affectedDomain = new NpcDomainBuilder("affectedDomain")
                .Select("Taking damage or dying")
                    .TakingDamageOrDying()  // Self contained task
                .Build();

            // Defining Regrouping domain
            var regroup = new NpcDomainBuilder("RegroupingDomain")
                .Sequence("Regrouping")
                    .Action("FindCluster").SetOperator(new FindNearestClusterOperator()).End()
                    .Action("GoToCluster").SetOperator(new MoveToTargetOperator(2f)).End()
                    .Action("Wander a bit").SetOperator(new WanderOperator(10f, 2, 1.5f))
                        // Giving stamina back only when done wandering
                        .IncrementState(NpcWorldState.StaminaLevel, 2, EffectType.PlanAndExecute)
                    .End()
                .End()
                .Build();

            // Total domain
            return new NpcDomainBuilder("Rioter")
                // High priority first
                .Select("Receive Effect from police actions (Damage, Stun, wtv)")
                    .Splice(affectedDomain)
                .End()
                .Sequence("To flee")
                    .PrimitiveTask<FindPolice>("Find closest police")
                        .HasStateGreaterThan(NpcWorldState.StaminaLevel, 2)
                        .HasState(NpcWorldState.PoliceInRange)
                    .End()
                    .Flee(NpcType.Police) // Self contained task
                        .DecrementState(NpcWorldState.StaminaLevel, 2, EffectType.PlanAndExecute)
                    .End()
                .End()
                .Select("Towards closest known destructible, or regroup")
                    .Sequence("To destructible")
                        .HasStateGreaterThan(NpcWorldState.StaminaLevel, 1) // Need at least a basic level of stamina
                        .HasState(NpcWorldState.HasDestructiblesInRange)
                        .PrimitiveTask<FindDestructible>("Find closest target").End()
                        .MoveToDestructible()    // Self contained Task
                        .PrimitiveTask<DamageDestructible>("Deal damage to target")
                            .DecrementState(NpcWorldState.StaminaLevel, EffectType.PlanAndExecute)
                        .End()
                    .End()
                    .Sequence("Enough stamina, but no destructible in sight.")
                        .Action("FindCluster").SetOperator(new FindNearestClusterOperator(103))
                            .HasStateGreaterThan(NpcWorldState.StaminaLevel, 6)    // Stop receiving stamina if more than 6 currently.
                        .End()
                        .Action("GoToCluster").SetOperator(new MoveToTargetOperator(3f)).End()
                        .Action("Randomize position to spot closest building").SetOperator(new WanderOperator(20f, 3, 0.8f)).End()
                    .End()
                    .Splice(regroup)    // Regrouping subdomain defined above
                .End()
                .Build();
        }

    }
}