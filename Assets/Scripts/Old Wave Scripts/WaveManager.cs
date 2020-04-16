using System.Collections;
using System.Collections.Generic;
//using SnapSystem;
using UnityEngine;
using UnityEngine.Events;

enum Phase {
    Market,
    Battle
};

/// <summary>
/// Controlling spawners, and events such as start/end of waves and others.
/// Count waves and number of expected ennemies per waves.
/// </summary>
public class WaveManager : MonoBehaviour {

    [SerializeField] private int _maxWaves;
    [SerializeField] private int _maxUnitsPerWave;
    [SerializeField] private int unitsPerWaveIncrement = 5;

    GameObject[]      _spawners;
    GameObject[]      _splines;

    [SerializeField]
    public List<Wave> waves;

    public UnityEvent onWaveStart;
    public UnityEvent onMapEnd;
    public UnityEvent onRoundEnd;
    public UnityEvent onMapLoad;

    private int  _waveNumber;
    private int  _unitsSpawned;
    bool         waveStopped;

    private bool _roundEnded = false;
    //private SnapManager _snapManager;
    //private LevelManager _levelManager;

    private int numOfStoppedWaves = 0;
    private int completedWaves = 0;
    private Phase phase;


    void Start() {
        _waveNumber = 1;
        onMapLoad.Invoke();
        //_snapManager = FindObjectOfType<SnapManager>();
        //_levelManager = FindObjectOfType<LevelManager>();
        _spawners = GameObject.FindGameObjectsWithTag("Spawner");
        _splines  = GameObject.FindGameObjectsWithTag("Spline");
        _maxWaves *= _spawners.Length;
        phase = Phase.Market;

        foreach(GameObject spawner in _spawners) {
            Wave w = spawner.GetComponent<Wave>();
            w._maxWaves = _maxWaves;
            w._maxUnitsPerWave = _maxUnitsPerWave;
            w._unitsPerWaveIncrement = unitsPerWaveIncrement;
            w._unitsSpawned = 0;
            w._waveStopped = true;
            
            waves.Add(w);
        }
    }


    void Update() {
        if(phase == Phase.Battle) {
            foreach(Wave w in waves) {
                if(!w._waveStopped) {
                    if(w._unitsSpawned >= w._maxUnitsPerWave) {
                        w.CancelSpawn();
                        w._waveStopped = true;
                        completedWaves++;
                    }
                }
                else {
                    if(w._spline.transform.childCount == 0)
                        numOfStoppedWaves++;
                }
            }

            // if no enemies left on the map, end the round -> go to market phase
            if(!_roundEnded) {
                if(numOfStoppedWaves == waves.Count) {
                    numOfStoppedWaves = 0;
                    _roundEnded = true;
                    _waveNumber++;
                    //_snapManager.UnlockGrid();
                    onRoundEnd?.Invoke();
                    phase = Phase.Market;
                }
                else {
                    numOfStoppedWaves = 0;
                }
            }

            // transition to next level
            if(_roundEnded && completedWaves == _maxWaves) {
                onMapEnd?.Invoke();
            }
        }
    }


    public int GetWaveNumber() { return _waveNumber; }

    /// <summary>
    /// Starts a wave that will spawn ennemies over multiple updates
    /// </summary>
    public void StartWave() {
        phase = Phase.Battle;

        StartCoroutine(StartSpawners());

        onWaveStart?.Invoke();
        //_snapManager.LockGrid();
        _roundEnded = false;
    }

    /// <summary>
    /// Triggering the waves with current amounts
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartSpawners() {
        int offset = 0;

        // Spawn boss every 5th wave
        if(_waveNumber % 5 == 0) {
            waves[0].SpawnBoss();
            yield return new WaitForSeconds(1);
        }

        foreach(Wave w in waves) {
            w._unitsSpawned = 0;
            w._maxUnitsPerWave += w._unitsPerWaveIncrement;
            w._waveStopped = false;
            w.InvokeSpawn(0 + offset, waves.Count);
            offset++;
        }

        yield return null;
    }
}