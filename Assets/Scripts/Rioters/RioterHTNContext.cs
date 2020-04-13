using System;
using System.Collections.Generic;
using FluidHTN;
using FluidHTN.Contexts;
using FluidHTN.Debug;
using FluidHTN.Factory;

namespace Rioters {

    public class RioterHTNContext : BaseContext {

        public override IFactory                          Factory          { get; set; } = new DefaultFactory();
        public override List<string>                      MTRDebug         { get; set; } = null;
        public override List<string>                      LastMTRDebug     { get; set; } = null;
        public override bool                              DebugMTR         { get; }      = false;
        public override Queue<IBaseDecompositionLogEntry> DecompositionLog { get; set; } = null;
        public override bool                              LogDecomposition { get; }      = false;

        private         byte[] _worldState = new byte[Enum.GetValues(typeof(RiotersWorldState)).Length];
        public override byte[] WorldState => _worldState;

        #region context blackboard manipulation

        public bool HasState(RiotersWorldState state, bool value) { return HasState((int) state, (byte) (value ? 1 : 0)); }

        public bool HasState(RiotersWorldState state, byte value) { return HasState((int) state, value); }

        public bool HasState(RiotersWorldState state) { return HasState((int) state, 1); }

        public void SetState(RiotersWorldState state, bool value, EffectType type) { SetState((int) state, (byte) (value ? 1 : 0), true, type); }

        public void SetState(RiotersWorldState state, byte value, EffectType type) { SetState((int) state, value, true, type); }

        public byte GetState(RiotersWorldState state) { return GetState((int) state); }

        #endregion

    }
}