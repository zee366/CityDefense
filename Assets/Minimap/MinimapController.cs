using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{

    public Camera Cam;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateRender", 0.0f, 1.0f);
    }

    IEnumerator UpdateRender()
    {
        Cam.Render();
        yield return null;
    }
}
