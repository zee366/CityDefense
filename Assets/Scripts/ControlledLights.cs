using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledLights : MonoBehaviour
{

    public Material light;
    public MeshRenderer _meshRenderer;
    private Coroutine _coroutine;

    private bool shouldTurnOn;
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer              = GetComponent<MeshRenderer>();
        _meshRenderer.materials[0] = Instantiate(light);
        _meshRenderer.materials[0].DisableKeyword("_EMISSION");
        shouldTurnOn = Random.Range(0, 100) > 25;
        if(shouldTurnOn) turnOn();
        _coroutine = StartCoroutine("Run");
    }

    public void turnOn() {
        _meshRenderer.materials[0].EnableKeyword("_EMISSION");
    }


    public void turnOff() {
        _meshRenderer.materials[0].DisableKeyword("_EMISSION");
    }
    
    IEnumerator Run() {
        while(true) {
            float timer = Random.Range(0, 100);
            yield return new WaitForSeconds(timer);
            turnOn();
            float timer2 = Random.Range(60, 120);
            yield return new WaitForSeconds(timer2);
            turnOff();
        }
    }
    
}
