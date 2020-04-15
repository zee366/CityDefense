using System;
using System.Collections.Generic;
using FluidHTN;
using FluidHTN.Contexts;
using FluidHTN.Debug;
using FluidHTN.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace Rioters {
    public class NpcHtnContext : BaseContext {

        public MonoBehaviour Agent    { get; }
        public NavMeshAgent  NavAgent { get; set; }

        public Vector3 Position {
            get { return Agent.transform.position; }
        }

        public List<Transform> policesInRange = new List<Transform>();
        public Transform ClosestPolice { get; set; }

        public List<Destructible> destructiblesInRange = new List<Destructible>();
        public Destructible       CurrentTarget { get; set; }

        public override IFactory                          Factory          { get; set; } = new DefaultFactory();
        public override List<string>                      MTRDebug         { get; set; } = null;
        public override List<string>                      LastMTRDebug     { get; set; } = null;
        public override bool                              DebugMTR         { get; }      = false;
        public override Queue<IBaseDecompositionLogEntry> DecompositionLog { get; set; } = null;
        public override bool                              LogDecomposition { get; }      = true;

        private         byte[] _worldState = new byte[Enum.GetValues(typeof(NpcWorldState)).Length];
        public override byte[] WorldState => _worldState;

        public NpcHtnContext(MonoBehaviour agent) { Agent = agent; }

        #region context blackboard manipulation

        public bool HasState(NpcWorldState state, bool value) { return HasState((int) state, (byte) (value ? 1 : 0)); }

        public bool HasState(NpcWorldState state, byte value) { return HasState((int) state, value); }

        public bool HasState(NpcWorldState state) { return HasState((int) state, 1); }

        public void SetState(NpcWorldState state, bool value, EffectType type) { SetState((int) state, (byte) (value ? 1 : 0), true, type); }

        public void SetState(NpcWorldState state, byte value, EffectType type) { SetState((int) state, value, true, type); }

        public byte GetState(NpcWorldState state) { return GetState((int) state); }

        #endregion

    }
}