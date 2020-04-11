using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterRioter : MonoBehaviour
{
    float health;
    // Start is called before the first frame update
    void Start()
    {
        health = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            //play a death animation clip?
            Destroy(gameObject);
        }
    }
}
