using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DamageMapController : MonoBehaviour
{
    public Material Green;
    public Material Orange;
    public Material Yellow;
    public Material Red;
    public Material Brown;

    public float Destroyed_Offset = 1; 

    public Camera Cam;
    private List<Vector3> DestroyedPositions; 
    
    // Start is called before the first frame update
    private void Start()
    {
        InitMarkers();
        InvokeRepeating("UpdateRender", 0.0f, 1.0f);
    }

    private void UpdateRender()
    {
        DestroyMarkers();
        CreateMarkers();
        Cam.Render();
    }
    private void InitMarkers()
    {
        List<Destructible> destructibles = FindObjectsOfType<Destructible>().ToList();

        int counter = 0;
        foreach (var d in destructibles)
        {
            GameObject rect = GameObject.CreatePrimitive(PrimitiveType.Plane);
            rect.transform.parent = gameObject.transform;
            rect.GetComponent<MeshRenderer>().material = Brown;
            rect.transform.position = new Vector3(d.transform.position.x, gameObject.transform.position.y - Destroyed_Offset, d.transform.position.z);
            rect.name = "DestroyedMark " + counter;
            counter++;
        }
    }
    private void CreateMarkers()
    {
        Destructible[] destructibles = FindObjectsOfType<Destructible>();
        int counter = 0;
        foreach (var d in destructibles)
        {
            GameObject rect = GameObject.CreatePrimitive(PrimitiveType.Plane);
            
            rect.name = "Marker " + counter;
            counter++;
            
            rect.transform.position = new Vector3(d.transform.position.x, gameObject.transform.position.y, d.transform.position.z);
            
            rect.transform.parent = gameObject.transform;

            if(d.GetCurrentHealth() >= 0.75f*d.health)
                rect.GetComponent<MeshRenderer>().material = Green;
            else if(d.GetCurrentHealth() >= 0.50f*d.health)
                rect.GetComponent<MeshRenderer>().material = Yellow;
            else if (d.GetCurrentHealth() >= 0.25f * d.health)
                rect.GetComponent<MeshRenderer>().material = Orange;
            else
                rect.GetComponent<MeshRenderer>().material = Red;
        }
    }
    private void DestroyMarkers()
    {
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name.Contains("Marker"))
                Destroy(gameObj);
        }
    }
}
