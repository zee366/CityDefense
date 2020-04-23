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

    //PR logic
    //PR object
    private PublicRelations publicRelations;
    //report object
    //private GameObject reporter;
    private Reporter reporter;
    //report PR amplifier
    private int reporterPresentPRAmplifier;

    //various costs for police abilities
    [SerializeField]
    private int costForFireRubberBullets = 0;
    [SerializeField]
    private int costForFireLethalBullets = 0;
    [SerializeField]
    private int costForSmokeGrenade = 0;
    [SerializeField]
    private int costForWaterCannon = 0;
    [SerializeField]
    private int costForReinforcement = 0;

    //various points for changing PR rate
    [SerializeField]
    private int arrestPR = 0;
    [SerializeField]
    private int aggressiveArrestPR = 0;
    [SerializeField]
    private int firingRubberBulletsPR = 0;
    [SerializeField]
    private int firingLethalBulletsPR = 0;
    [SerializeField]
    private int smokeGrenadePR = 0;
    [SerializeField]
    private int waterCannonPR = 0;
    //Accessors and mutators for certain properties
    public int CostForFireBullets { get; set; }
    public int CostForSmokeGrenade { get => costForSmokeGrenade; set => costForSmokeGrenade = value; }
    public int CostForWaterCannon { get => costForWaterCannon; set => costForWaterCannon = value; }
    public int CostForReinforcement { get => costForReinforcement; set => costForReinforcement = value; }
    // Hand holding
    [Header("Hand")]
    public GameObject lamp;
    public GameObject gun;

    // Start is called before the first frame update
    void Start()
    {
        infoBubble = FindObjectOfType<ScreenSpaceTargetBubble>();
        publicRelations = FindObjectOfType<PublicRelations>();
        reporter = FindObjectOfType<Reporter>();
        reporterPresentPRAmplifier = 1;
        bulletSetType = bulletTypes[0];
        bulletDamage = 5.0f;
        CostForFireBullets = costForFireRubberBullets; //by default rubber bullet cost
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
        //reporter present logic
        //if reporter's physics overlap returns this PoliceAbilities object
        if (reporter.CheckIfNearPoliceAndRioters().Contains(this.gameObject))
            reporterPresentPRAmplifier = 3;

        //show appropriate arresting image based on success/fail
        if (rioterTarget && rioterTarget.health <= 25.0f)
        {
            //logic for less points deducted from PR
            publicRelations.ImprovePR(arrestPR * reporterPresentPRAmplifier);

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
        reporterPresentPRAmplifier = 1;
    }
    
    public void AggressiveArrest()
    {
        //reporter present logic
        if (reporter.CheckIfNearPoliceAndRioters().Contains(this.gameObject))
            reporterPresentPRAmplifier = 3;

        //show appropriate aggressive arresting image based on success/fail
        if (rioterTarget)
        {
            //logic for more points deducted from PR
            publicRelations.WorsenPR(aggressiveArrestPR * reporterPresentPRAmplifier);

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
        reporterPresentPRAmplifier = 1;
    }

    public void FireBullets()
    {
        //cost of action
        publicRelations.CostOfAction(CostForFireBullets);

        //reporter present logic
        if (reporter.CheckIfNearPoliceAndRioters().Contains(this.gameObject))
            reporterPresentPRAmplifier = 2;

        //logic for PR system cost
        if (bulletSetType == bulletTypes[0])
            publicRelations.WorsenPR(firingRubberBulletsPR * reporterPresentPRAmplifier);
        else
            publicRelations.WorsenPR(firingLethalBulletsPR * reporterPresentPRAmplifier);

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
        reporterPresentPRAmplifier = 1;
    }

    public void UseRubberBullets()
    {
        //change cost of firing bullets to be cost of firing rubber bullets
        CostForFireBullets = costForFireRubberBullets;

        bulletSetType = bulletTypes[0];
        bulletDamage = 5.0f;
    }

    public void UseSmokeGrenade()
    {
        //cost of action
        publicRelations.CostOfAction(costForSmokeGrenade);

        //reporter present logic
        if (reporter.CheckIfNearPoliceAndRioters().Contains(this.gameObject))
            reporterPresentPRAmplifier = 2;

        //logic for more points deducted from PR
        publicRelations.WorsenPR(smokeGrenadePR * reporterPresentPRAmplifier);

        //logic for throwing grenade at an arch
        GameObject sG = Instantiate(smokeGrenade, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
        sG.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);

        reporterPresentPRAmplifier = 1;
    }

    public void UseWaterCannon()
    {
        //cost of action
        publicRelations.CostOfAction(costForWaterCannon);

        //reporter present logic
        if (reporter.CheckIfNearPoliceAndRioters().Contains(this.gameObject))
            reporterPresentPRAmplifier = 2;
        Debug.Log("water cannon PR rate: " + waterCannonPR * reporterPresentPRAmplifier);
        //logic for PR system cost
        publicRelations.ImprovePR(waterCannonPR * reporterPresentPRAmplifier);

        //logic for using water cannon
        Instantiate(waterCannon, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
        reporterPresentPRAmplifier = 1;
    }

    public void UseLethalBullets()
    {
        //change cost of firing bullets to be cost of firing rubber bullets
        CostForFireBullets = costForFireLethalBullets;

        bulletSetType = bulletTypes[1];
        bulletDamage = 20.0f;
    }

    public void ReinforceSquad()
    {
        //cost of action
        publicRelations.CostOfAction(costForReinforcement);

        //logic for summoning a police squad member
        //spawn point is at donut shop
        //joins calling flock once they select a position
        Flock f = FindObjectOfType<Flock>();
        f.AddAgent();
    }

    public void equipWeapon() {
        lamp.SetActive(false);
        gun.SetActive(true);
    }
    public void equipLamp() {
        lamp.SetActive(true);
        gun.SetActive(false);
    }

}
