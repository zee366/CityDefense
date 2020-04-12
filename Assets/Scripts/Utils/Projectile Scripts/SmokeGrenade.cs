using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    public float timeDelay;
    float startTimer;

    float damageGiven;
    public float explosiveForce;
    public float explosiveForceRadius;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        startTimer = 0.0f;
        animator = GetComponent<Animator>();
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
        animator.Play("Smoke");
        GetComponent<MeshRenderer>().enabled = false;
        foreach (Collider c in col)
        {
            if (c.gameObject.GetComponent<TesterRioter>())
            {
                c.gameObject.GetComponent<TesterRioter>().TakeDamage(damageGiven);
                c.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosiveForce, transform.position, explosiveForceRadius);
            }
        }
        Destroy(gameObject);
    }
}
