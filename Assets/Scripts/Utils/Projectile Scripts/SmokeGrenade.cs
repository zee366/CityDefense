using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    public float timeDelay;
    float startTimer;

    public float damageGiven;
    public float explosiveForce;
    public float explosiveForceRadius;

    //Smoke smokePrefab
    ParticleSystem smokeParticleSystem;
    Animator smokeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        smokeParticleSystem = GetComponentInChildren<ParticleSystem>();
        smokeAnimator = GetComponentInChildren<Animator>();
        startTimer = 0.0f;
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        startTimer += Time.deltaTime;
        if (startTimer >= timeDelay)
            SmokeExplosion();
    }

    public void SmokeExplosion()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, explosiveForceRadius);
        GetComponent<MeshRenderer>().enabled = false;
        smokeParticleSystem.Play();
        smokeAnimator.Play("Smoke Animation");
        foreach (Collider c in col)
        {
            if (c.gameObject.GetComponent<RioterHealth>())
            {
                c.gameObject.GetComponent<RioterHealth>().TakeDamage(damageGiven);
                //c.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosiveForce, transform.position, explosiveForceRadius);
            }
        }
    }
}
