using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

/// <summary>
/// Controlling a wave for a single spawner.
/// Driven by the wave manager
/// </summary>
public class Wave : MonoBehaviour{
    public int _maxWaves;
    public int _maxUnitsPerWave;
    public int _unitsPerWaveIncrement;

    public int _unitsSpawned;
    public bool _waveStopped { get; set; }

    public GameObject _spawner;
    public GameObject _spline;

    /// <summary>
    /// Spawn one ennemy only
    /// </summary>
    public void SpawnEnemy() {
        _spawner.GetComponent<Spawner>().SpawnOne();
        _unitsSpawned++;
    }

    public void InvokeSpawn(float start, float repeat) {
        InvokeRepeating("SpawnEnemy", start, repeat);
    }

    /// <summary>
    /// Cancel repeated invocation
    /// </summary>
    public void CancelSpawn() {
        CancelInvoke("SpawnEnemy");
    }

    /// <summary>
    /// Triggers spawning of a boss on that spline
    /// </summary>
    public void SpawnBoss() {
        //_spawner.GetComponent<Spawner>().SpawnBoss();
    }
};
