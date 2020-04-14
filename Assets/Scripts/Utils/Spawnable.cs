using System;
using UnityEngine;

/// <summary>
/// Used as a Spawner entry to contain object to spawn with his probability of spawning
/// </summary>
[Serializable]
public struct Spawnable {

    public GameObject prefab;
    [Range(0, 100)] public float probability;

}