using FluidHTN;
using FluidHTN.Factory;
using FluidHTN.Operators;
using FluidHTN.PrimitiveTasks;
using Rioters.Conditions;
using Rioters.Effects;
using Rioters.Operators;

namespace Rioters {
    public class RiotersDomainBuilder : BaseDomainBuilder<RiotersDomainBuilder, RioterHTNContext> {

        public RiotersDomainBuilder(string domainName) : base(domainName, new DefaultFactory()) { }

        public RiotersDomainBuilder(string domainName, IFactory factory) : base(domainName, factory) { }


        public RiotersDomainBuilder HasState(RiotersWorldState state) {
            var condition = new HasWorldStateCondition(state);
            Pointer.AddCondition(condition);
            return this;
        }


        public RiotersDomainBuilder HasState(RiotersWorldState state, byte value) {
            var condition = new HasWorldStateCondition(state, value);
            Pointer.AddCondition(condition);
            return this;
        }


        public RiotersDomainBuilder HasStateGreaterThan(RiotersWorldState state, byte value) {
            var condition = new HasWorldStateGreaterThanCondition(state, value);
            Pointer.AddCondition(condition);
            return this;
        }


        public RiotersDomainBuilder SetState(RiotersWorldState state, EffectType type) {
            if ( Pointer is IPrimitiveTask task ) {
                var effect = new SetWorldStateEffect(state, type);
                task.AddEffect(effect);
            }

            return this;
        }


        public RiotersDomainBuilder SetState(RiotersWorldState state, bool value, EffectType type) {
            if ( Pointer is IPrimitiveTask task ) {
                var effect = new SetWorldStateEffect(state, value, type);
                task.AddEffect(effect);
            }

            return this;
        }


        public RiotersDomainBuilder SetState(RiotersWorldState state, byte value, EffectType type) {
            if ( Pointer is IPrimitiveTask task ) {
                var effect = new SetWorldStateEffect(state, value, type);
                task.AddEffect(effect);
            }

            return this;
        }


        public RiotersDomainBuilder SetOperator(IOperator op) {
            if ( Pointer is IPrimitiveTask task ) {
                task.SetOperator(op);
            }

            return this;
        }


        public RiotersDomainBuilder MoveToDestructible() {
            Action("Move to building");
            if ( Pointer is IPrimitiveTask task ) {
                task.SetOperator(new MoveToDestructibleOperator());
            }

            SetState(RiotersWorldState.TargetInAttackRange, EffectType.PlanAndExecute);
            End();
            return this;
        }

    }
}