using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Roadlight : MonoBehaviour {

    public Material greenLight;
    public Material redLight;

    private bool state = false;

    private int counter;
    // Start is called before the first frame update
    void Awake() {
        greenLight.DisableKeyword("_EMISSION");
        redLight.DisableKeyword("_EMISSION");
    }


    public void ToggleLight() {
        if ( state ) {
            greenLight.EnableKeyword ("_EMISSION");
            redLight.DisableKeyword ("_EMISSION");
        } else {
            redLight.EnableKeyword ("_EMISSION");
            greenLight.DisableKeyword ("_EMISSION");
        }
        state = !state;
    } 
    
    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;
        if ( counter <= 60 ) return;

        counter = 0;
        ToggleLight();
    }
}
