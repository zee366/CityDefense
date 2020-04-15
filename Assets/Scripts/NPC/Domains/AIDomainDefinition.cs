using FluidHTN;
using UnityEngine;

namespace Rioters {
    public abstract class AIDomainDefinition : ScriptableObject {

        public abstract Domain<NpcHtnContext> Create();

    }
}