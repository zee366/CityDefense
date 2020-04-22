using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UI;
using UnityEngine.UI;

public class PoliceAbilities : MonoBehaviour
{
    //change to actual class of it? even needed? 
    //GameObject policeSquadLeader;

    //rioters
    //specify class
    public RioterHealth rioterTarget;
    public float targetRadius;

    //Grenade prefab
    public GameObject smokeGrenade;
    public float launchForce;

    //bullets
    //specify class?
    public List<GameObject> bulletTypes;
    GameObject bulletSetType;
    float bulletDamage;

    //water cannon
    public GameObject waterCannon;

    //arresting images and info bubble
    public Sprite arrestImg;
    public Sprite failedArrestImg;
    public Sprite aggressiveArrestImg;
    public Sprite failedAggressiveArrestImg;
    ScreenSpaceTargetBubble infoBubble;




    // Start is called before the first frame update
    void Start()
    {
        infoBubble = FindObjectOfType<ScreenSpaceTargetBubble>();
        bulletSetType = bulletTypes[0];
        bulletDamage = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, targetRadius);

        foreach (Collider c in col)
        {
            if (c.gameObject.GetComponent<RioterHealth>())
            {
                rioterTarget = c.gameObject.GetComponent<RioterHealth>();
                break;
            }
            rioterTarget = null;
        }
    }

    

    public void Arrest()
    {
        //logic for less points deducted from PR
        //...

        //show appropriate arresting image based on success/fail
        if (rioterTarget && rioterTarget.health <= 25.0f)
        {
            infoBubble.SetFramingImage(arrestImg);
            infoBubble.Open();
            rioterTarget.TakeDamage(100.0f);
            infoBubble.Close();
        }
        else
        {
            infoBubble.SetFramingImage(failedArrestImg);
            infoBubble.Open();
            infoBubble.Close();
        }
    }
    
    public void AggressiveArrest()
    {
        //logic for more points deducted from PR
        //...

        //show appropriate arresting image based on success/fail
        if (rioterTarget)
        {
            infoBubble.SetFramingImage(aggressiveArrestImg);
            infoBubble.Open();
            rioterTarget.TakeDamage(100.0f);
            infoBubble.Close();
        }
        else
        {
            infoBubble.SetFramingImage(failedAggressiveArrestImg);
            infoBubble.Open();
            infoBubble.Close();
        }
    }

    public void FireBullets()
    {
        //logic for PR system cost
        //...

        //logic for firing bullets
        if (rioterTarget)
        {
            Vector3 start = transform.position;
            Vector3 direction = rioterTarget.transform.position - start;
            float distance = direction.magnitude;

            //get the layermask of the rioter
            int layerMask = 1 << 11;

            //invert this layermask to collide with everything, but the player
            layerMask = ~layerMask;

            //check if we hit something else besides the rioter
            //if no such collision, then check if the player is visible from the front of 
            //the zombie tank and if so, then fire (0.2f was tested to be front enough)
            RaycastHit hit;
            if (direction != Vector3.zero)
            {
                if (!Physics.Raycast(start, direction.normalized, out hit, distance, layerMask))
                {
                    rioterTarget.TakeDamage(bulletDamage);
                }
            }
        }
    }

    public void UseRubberBullets()
    {
        //logic for PR system cost
        //...

        bulletSetType = bulletTypes[0];
        bulletDamage = 5.0f;
    }

    public void UseSmokeGrenade()
    {
        //logic for throwing grenade at an arch
        GameObject sG = Instantiate(smokeGrenade, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
        sG.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);
    }

    public void UseWaterCannon()
    {
        //logic for PR system cost
        //...

        //logic for using water cannon
        Instantiate(waterCannon, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
    }

    public void UseLethalBullets()
    {
        //logic for PR system cost
        //...

        bulletSetType = bulletTypes[1];
        bulletDamage = 20.0f;
    }

    public void ReinforceSquad()
    {
        //logic for PR system cost
        //...

        //logic for summoning a police squad member
        //spawn point is at donut shop
        //joins calling flock once they select a position
        //make sure they're not an Agent 0...
        //TODO? make sure they avoid rioters on way to calling flock?
        Flock f = FindObjectOfType<Flock>();
        f.AddAgent();
    }
}
