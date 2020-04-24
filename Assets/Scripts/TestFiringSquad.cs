using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFiringSquad : MonoBehaviour
{
    public FiringSquad fs;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            fs.Fire(target.position);
    }
}
