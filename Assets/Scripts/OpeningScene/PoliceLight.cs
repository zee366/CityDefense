using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLight : MonoBehaviour
{
    public float angle;

    void Update()
    {
        transform.Rotate(Vector3.up, angle * Time.deltaTime, Space.World);
    }
}
