using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMapController : MonoBehaviour
{
    public Material green;
    public Material orange;
    public Material yellow;
    public Material red;

    public Camera Cam;

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("UpdateRender", 0.0f, 1.0f);
    }

    private void UpdateRender()
    {
        DrawDestructibles();
        Cam.Render();
        EraseDestructibles();
    }

    private void DrawDestructibles()
    {
        Destructible[] Destructibles = FindObjectsOfType<Destructible>();
        int counter = 0;
        foreach (var d in Destructibles)
        {
            GameObject rect = GameObject.CreatePrimitive(PrimitiveType.Plane);
            rect.name = "Marker " + counter;
            counter++;
            rect.transform.position = new Vector3(d.transform.position.x, gameObject.transform.position.y, d.transform.position.z);

            if(d.GetCurrentHealth() > 0.75f*d.health)
                rect.GetComponent<MeshRenderer>().material = green;
            else if(d.GetCurrentHealth() > 0.50f*d.health)
                rect.GetComponent<MeshRenderer>().material = orange;
            else if (d.GetCurrentHealth() > 0.25f * d.health)
                rect.GetComponent<MeshRenderer>().material = yellow;
            else if (d.GetCurrentHealth() >= 0f * d.health)
                rect.GetComponent<MeshRenderer>().material = red;
        }
    }
    private void EraseDestructibles()
    {
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name.Contains("Marker"))
            {
                Destroy(gameObj);
            }
        }
    }
}
