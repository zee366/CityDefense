using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    public Light light;
    public float startingIntensity;
    public float minimumIntensity;
    [Range(1,10)]
    public float rate;
    private float down;
    // Start is called before the first frame update
    void Start() { light.intensity = startingIntensity; }

    // Update is called once per frame
    void FixedUpdate() {
        down += rate / 1000000;
        var newIntensity = light.intensity - down;
        if ( newIntensity < minimumIntensity ) newIntensity = minimumIntensity;
        light.intensity = newIntensity;
    }
}
