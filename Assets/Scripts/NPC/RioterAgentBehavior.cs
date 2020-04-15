using FluidHTN;
using UnityEngine;
using UnityEngine.AI;

namespace Rioters {
    [RequireComponent(typeof(NavMeshAgent))]
    public class RioterAgentBehavior : MonoBehaviour {

        [SerializeField]
        private AIDomainDefinition _domainDefinition;

        [SerializeField]
        [Tooltip("Time interval between each surroundings sensing procedure.")]
        private float _sensingInterval = 1f;

        [SerializeField]
        private float _sensingRange = 10f;

        private Planner<NpcHtnContext> _planner;
        private Domain<NpcHtnContext>  _domain;
        private NpcHtnContext          _context;
        private float                  _currentSensingTimer;
        private Collider[]             _sensedColliders = new Collider[128];


        void Awake() {
            _currentSensingTimer = _sensingInterval;
            _planner             = new Planner<NpcHtnContext>();
            _domain              = _domainDefinition.Create();

            _context                    = new NpcHtnContext(this);
            _context.NavAgent           = GetComponent<NavMeshAgent>();
            _context.NavAgent.isStopped = true;
            _context.Init();
        }


        void Update() {
            SenseSurroundings();

            _planner.Tick(_domain, _context);
        }


        private void SenseSurroundings() {
            _currentSensingTimer += Time.deltaTime;
            if ( _currentSensingTimer < _sensingInterval ) return;

            _context.destructiblesInRange.Clear();

            // Sense
            int count = Physics.OverlapSphereNonAlloc(transform.position, _sensingRange, _sensedColliders);

            for ( int i = 0; i < count; i++ ) {
                // Check for destructibles
                if ( _sensedColliders[i].TryGetComponent(out Destructible destructible) && !destructible.IsDead )
                    _context.destructiblesInRange.Add(destructible);

                // Check for Police
                if ( _sensedColliders[i].TryGetComponent(out PoliceAbilities p) )
                    _context.policesInRange.Add(_sensedColliders[i].transform);
            }

            // Change world state
            _context.SetState(NpcWorldState.HasDestructiblesInRange, _context.destructiblesInRange.Count > 0, EffectType.Permanent);

            _currentSensingTimer = 0;
        }

    }
}