using FluidHTN;
using FluidHTN.Factory;
using FluidHTN.Operators;
using FluidHTN.PrimitiveTasks;
using Rioters.Conditions;
using Rioters.Effects;
using Rioters.Operators;

namespace Rioters {
    public class NpcDomainBuilder : BaseDomainBuilder<NpcDomainBuilder, NpcHtnContext> {

        public NpcDomainBuilder(string domainName) : base(domainName, new DefaultFactory()) { }

        public NpcDomainBuilder(string domainName, IFactory factory) : base(domainName, factory) { }


        public NpcDomainBuilder HasState(NpcWorldState state) {
            var condition = new HasWorldStateCondition(state);
            Pointer.AddCondition(condition);
            return this;
        }


        public NpcDomainBuilder HasState(NpcWorldState state, byte value) {
            var condition = new HasWorldStateCondition(state, value);
            Pointer.AddCondition(condition);
            return this;
        }


        public NpcDomainBuilder HasStateGreaterThan(NpcWorldState state, byte value) {
            var condition = new HasWorldStateGreaterThanCondition(state, value);
            Pointer.AddCondition(condition);
            return this;
        }


        public NpcDomainBuilder SetState(NpcWorldState state, EffectType type) {
            if ( Pointer is IPrimitiveTask task ) {
                var effect = new SetWorldStateEffect(state, type);
                task.AddEffect(effect);
            }

            return this;
        }


        public NpcDomainBuilder SetState(NpcWorldState state, bool value, EffectType type) {
            if ( Pointer is IPrimitiveTask task ) {
                var effect = new SetWorldStateEffect(state, value, type);
                task.AddEffect(effect);
            }

            return this;
        }


        public NpcDomainBuilder SetState(NpcWorldState state, byte value, EffectType type) {
            if ( Pointer is IPrimitiveTask task ) {
                var effect = new SetWorldStateEffect(state, value, type);
                task.AddEffect(effect);
            }

            return this;
        }


        public NpcDomainBuilder SetOperator(IOperator op) {
            if ( Pointer is IPrimitiveTask task ) {
                task.SetOperator(op);
            }

            return this;
        }


        public NpcDomainBuilder MoveToDestructible() {
            Action("Move to building");
            if ( Pointer is IPrimitiveTask task ) {
                task.SetOperator(new MoveToDestructibleOperator());
            }

            SetState(NpcWorldState.TargetInAttackRange, EffectType.PlanAndExecute);
            End();
            return this;
        }


        public NpcDomainBuilder Flee(NpcType type) {
            Action("Flee from Npc type: "+type);
            if ( Pointer is IPrimitiveTask task ) {
                // task.SetOperator(new MoveToDestructibleOperator());
            }

            End();
            return this;
        }

    }
}