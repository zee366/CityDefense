using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reporter : MonoBehaviour
{
    public List<GameObject> listOfRiotersAndPolice;
    [SerializeField]
    private float mediaSceneRadius = 0;

    //will have a physics overlap logic

    // Start is called before the first frame update
    void Start()
    {
        listOfRiotersAndPolice = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        listOfRiotersAndPolice = CheckIfNearPoliceAndRioters();
    }

    public List<GameObject> CheckIfNearPoliceAndRioters()
    {
        listOfRiotersAndPolice.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, mediaSceneRadius);

        foreach (Collider cd in colliders)
        {
            if (cd.gameObject.GetComponent<PoliceAbilities>())
                listOfRiotersAndPolice.Add(cd.gameObject);
            else if (cd.gameObject.GetComponent<Rioters.RioterAgentBehavior>())
                listOfRiotersAndPolice.Add(cd.gameObject);
        }

        return listOfRiotersAndPolice;
    }
}
