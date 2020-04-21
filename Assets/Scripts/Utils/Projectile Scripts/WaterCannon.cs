using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCannon : MonoBehaviour
{
    bool waterCannonActivated;
    float waterCDeathTimer;
    float waterCMaxDeathTimer;

    // Start is called before the first frame update
    void Start()
    {
        //waterCannonActivated = true;
        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
