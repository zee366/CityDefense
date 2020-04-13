using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        // Make sure the camera is disabled in the Minimap Component prefab! Else performance will be very poor! 
        cam.Render();     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
