using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceHealth : MonoBehaviour
{
    float health;
    UIPoliceAbilities policeSquadUIListAccess;
    Flock policeSquadFlockListAccess;
    // Start is called before the first frame update
    void Start()
    {
        health = 100.0f;
        policeSquadUIListAccess = FindObjectOfType<UIPoliceAbilities>();
        policeSquadFlockListAccess = FindObjectOfType<Flock>();
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
            policeSquadUIListAccess.policeSquad.Remove(gameObject.GetComponent<PoliceAbilities>());
            policeSquadFlockListAccess.agents.Remove(gameObject.GetComponent<FlockAgent>());
            Destroy(gameObject);
        }
    }
}
