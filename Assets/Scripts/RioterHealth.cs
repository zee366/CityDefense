using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RioterHealth : MonoBehaviour
{
    public float health;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        health = 100.0f;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        CheckDead();
    }

    public void CheckDead()
    {
        if (health <= 0)
        {
            Debug.Log("I'm dying");
            //play a death animation clip?
            Destroy(gameObject);
        }
    }
}
