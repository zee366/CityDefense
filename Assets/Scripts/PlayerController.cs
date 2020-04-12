using UnityEngine;
using UnityEngine.AI;


// This script is only ever used in R1 for Unity's navmesh. 

public class PlayerController : MonoBehaviour
{
    public new Camera camera;

    public NavMeshAgent agent;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}