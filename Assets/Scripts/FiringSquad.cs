using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringSquad : MonoBehaviour
{
    public ParticleSystem[] particleSystems;

    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    public void Fire(Vector3 position) {
        foreach(ParticleSystem ps in particleSystems) {
            ps.transform.rotation = Quaternion.FromToRotation(Vector3.forward, position - transform.position);
            ps.Play();
        }
    }
}
