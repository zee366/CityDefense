using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RioterHealth : MonoBehaviour
{
    public float health;

    public bool IsDead { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        IsDead = false;
        health = 100.0f;
    }

    public void TakeDamage(float amount)
    {
        if (health <= 0.0f)
        {
            health = 0.0f;
            IsDead = true;
        }
        else
            health -= amount;
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
