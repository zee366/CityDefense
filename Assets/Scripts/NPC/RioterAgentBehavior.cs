using System.Collections.Generic;
using FluidHTN;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Rioters {
    [RequireComponent(typeof(NavMeshAgent))]
    public class RioterAgentBehavior : MonoBehaviour {

        [SerializeField]
        private AIDomainDefinition _domainDefinition;

        [SerializeField]
        [Tooltip("Time interval between each surroundings sensing procedure.")]
        private float _sensingInterval = 0.2f;

        [SerializeField]
        private float _sensingRange = 10f;

        [SerializeField]
        private float _damagePerSecond = 2f;

        private Planner<NpcHtnContext> _planner;
        private Domain<NpcHtnContext>  _domain;
        private NpcHtnContext          _context;
        private float                  _currentSensingTimer;
        private Collider[]             _sensedColliders = new Collider[128];

        public bool logPlan = false;

        void Awake() {
            _currentSensingTimer = _sensingInterval;
            _planner             = new Planner<NpcHtnContext>();
            _domain              = _domainDefinition.Create();

            // Constructing initial context
            _context                      = new NpcHtnContext(this);
            _context.anim                 = GetComponent<Animator>();
            _context.NavAgent             = GetComponent<NavMeshAgent>();
            _context.NavAgent.isStopped   = true;
            _context.DPS                  = _damagePerSecond;
            _context.rioterHealth         = GetComponent<RioterHealth>();
            _context.SetState(NpcWorldState.StaminaLevel, 2, EffectType.Permanent);

            if ( transform.parent != null && transform.parent.TryGetComponent(out DynamicClustersApproximator clusterManager) ) {
                _context.ClustersApproximator = clusterManager;
            } else {
                Debug.LogError("Rioter NPC needs to be spawned as a child of a DynamicClustersApproximator container.");
                #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
                #endif
            }

            _context.Init();


            _planner.OnNewPlan += OnNewPlanTest;
        }


        private void OnNewPlanTest(Queue<ITask> obj) {
            if ( logPlan ) {
                Debug.Log("====== New Plan created");
                foreach ( ITask task in obj ) {
                    Debug.Log(task.Name);
                }
                Debug.Break();
            }
        }


        void Update() {
            SenseSurroundings();

            _context.Verbose = logPlan;
            _planner.Tick(_domain, _context);
        }

        private void SenseSurroundings() {
            _currentSensingTimer += Time.deltaTime;
            if ( _currentSensingTimer < _sensingInterval ) return;

            _context.destructiblesInRange.Clear();
            _context.policesInRange.Clear();

            // Sense
            int count = Physics.OverlapSphereNonAlloc(transform.position, _sensingRange, _sensedColliders);

            for ( int i = 0; i < count; i++ ) {
                // Check for destructibles
                if ( _sensedColliders[i].TryGetComponent(out Destructible destructible) && !destructible.IsDead )
                    _context.destructiblesInRange.Add(destructible);

                // Check for Police
                if ( _sensedColliders[i].TryGetComponent(out PoliceAbilities p) )
                    _context.policesInRange.Add(p.transform);
            }

            // Change world state
            _context.SetState(NpcWorldState.HasDestructiblesInRange, _context.destructiblesInRange.Count > 0, EffectType.Permanent);
            _context.SetState(NpcWorldState.PoliceInRange,           _context.policesInRange.Count > 0,       EffectType.Permanent);

            _currentSensingTimer = 0;
        }

    }
}