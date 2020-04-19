using FluidHTN;
using FluidHTN.Factory;
using FluidHTN.Operators;
using FluidHTN.PrimitiveTasks;
using Rioters.Conditions;
using Rioters.Effects;
using Rioters.Operators;
using UnityEngine;

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


        public NpcDomainBuilder IncrementState(NpcWorldState state, EffectType type) { return IncrementState(state, 1, type); }


        public NpcDomainBuilder IncrementState(NpcWorldState state, byte value, EffectType type) {
            if ( Pointer is IPrimitiveTask task ) {
                var effect = new IncrementWorldStateEffect(state, value, type);
                task.AddEffect(effect);
            }

            return this;
        }

        public NpcDomainBuilder DecrementState(NpcWorldState state, EffectType type) { return DecrementState(state, 1, type); }


        public NpcDomainBuilder DecrementState(NpcWorldState state, byte value, EffectType type) {
            if ( Pointer is IPrimitiveTask task ) {
                var effect = new DecrementWorldStateEffect(state, value, type);
                task.AddEffect(effect);
            }

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

            End();
            return this;
        }


        public NpcDomainBuilder Flee(NpcType type, float refreshInterval = 2f) {
            Action("Flee from Npc type: " + type);
            if ( Pointer is IPrimitiveTask task ) {
                task.SetOperator(new FleeOperator(type, refreshInterval));
            }

            return this;
        }


    }
}