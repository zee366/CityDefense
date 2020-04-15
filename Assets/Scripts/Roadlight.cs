using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Roadlight : MonoBehaviour {

    public Material greenLight;
    public Material redLight;
    public MeshRenderer _meshRenderer;
    private int counter;
    // Start is called before the first frame update
    void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.materials[2] = Instantiate(redLight);
        _meshRenderer.materials[6] = Instantiate(greenLight);

        _meshRenderer.materials[6].DisableKeyword("_EMISSION");
        _meshRenderer.materials[2].DisableKeyword("_EMISSION");
    }


    public void turnOnGreenLight() {
        _meshRenderer.materials[6].EnableKeyword("_EMISSION");
        _meshRenderer.materials[2].DisableKeyword("_EMISSION");
    }


    public void turnOnRedLight() {
        _meshRenderer.materials[2].EnableKeyword("_EMISSION");
        _meshRenderer.materials[6].DisableKeyword("_EMISSION");
    }
}
