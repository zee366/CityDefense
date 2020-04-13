using FluidHTN;
using UnityEngine;

namespace Rioters {
    public class RioterAgentBehavior : MonoBehaviour {

        [SerializeField]
        private AIDomainDefinition _domainDefinition;

        private Planner<RioterHTNContext> _planner;
        private Domain<RioterHTNContext> _domain;
        private RioterHTNContext _context;

        void Awake() {
            _planner = new Planner<RioterHTNContext>();
            _domain = _domainDefinition.Create();
            _context = new RioterHTNContext();
        }


        void Update() {
            _planner.Tick(_domain, _context);
        }

    }
}