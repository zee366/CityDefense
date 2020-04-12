using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PoliceAbilities : MonoBehaviour
{
    //change to actual class of it? even needed? 
    //GameObject policeSquadLeader;

    //rioters
    //specify class
    public TesterRioter rioterTarget;

    //Grenade prefab
    public GameObject smokeGrenade;
    public float launchForce;

    //bullets
    //specify class?
    public List<GameObject> bulletTypes;
    GameObject bulletSetType;

    //water cannon
    public GameObject waterCannon;
    





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void Arrest()
    {
        //logic for less points deducted from PR

        //have an animation for the arrest?
        Destroy(rioterTarget);
    }
    
    public void AggressiveArrest()
    {
        //logic for more points deducted from PR

        //have an animation for the arrest?
        Destroy(rioterTarget);
    }

    private void FireBullets()
    {

    }

    public void UseRubberBullets()
    {
        bulletSetType = bulletTypes[0];
    }

    public void UseSmokeGrenade()
    {
        //logic for throwing grenade at an arch
        GameObject sG = Instantiate(smokeGrenade, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
        sG.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);
    }

    public void UseWaterCannon()
    {
        //logic for using water cannon
        Instantiate(waterCannon, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
        //GameObject sG = Instantiate(waterCannon, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
    }

    public void UseLethalBullets()
    {
        bulletSetType = bulletTypes[1];
    }

    public void ReinforceSquad()
    {
        //logic for summoning a police squad member

    }
}
