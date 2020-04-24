using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPoliceAbilities : MonoBehaviour
{
    //public PoliceAbilities policeSquadLeader;
    public List<PoliceAbilities> policeSquad;
    Flock flock;

    public Image smokeGButtonImg;
    float smokeGTimer;
    bool smokeGrenadeCoolDown;

    public Image waterCButtonImg;
    float waterCTimer;
    bool waterCannonCoolDown;

    public Image reinforceButtonImg;
    float reinforceTimer;
    bool reinforceCoolDown;

    //PR object
    private PublicRelations publicRelations;

    //Disabling button UI system
    private bool rubberBulletsSelected;
    private bool lethalBulletsSelected;
    public Button rubberBullets;
    public Button lethalBullets;
    public Button fireBulletButton;
    public Button smokeGButton;
    public Button waterCButton;
    public Button reinforcementButton;



    // Start is called before the first frame update
    void Start()
    {
        publicRelations = FindObjectOfType<PublicRelations>();
        
        flock = FindObjectOfType<Flock>();
        smokeGTimer = 5.5f;
        smokeGrenadeCoolDown = false;

        waterCTimer = 3.0f;
        waterCannonCoolDown = false;

        reinforceTimer = 2.0f;
        reinforceCoolDown = false;

        rubberBulletsSelected = true;
        lethalBulletsSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (policeSquad.Count != flock.agents.Count)
        {
            //removal of a squad member
            for(int i = 0; i < policeSquad.Count; i++)
            {
                if (!flock.agents.Contains(policeSquad[i].gameObject.GetComponent<FlockAgent>()))
                    policeSquad.Remove(policeSquad[i]);
            }

            //addition of a squad member
            foreach (FlockAgent fA in flock.agents)
            {
                PoliceAbilities pA = fA.gameObject.GetComponent<PoliceAbilities>();
                if (fA.partOfFlock)
                {
                    if (!policeSquad.Contains(pA))
                        policeSquad.Add(fA.gameObject.GetComponent<PoliceAbilities>());
                }
            }
        }

        CheckWhaBulletIsSelected(rubberBullets, lethalBullets);

        CheckIfEnoughFundingForButtons(fireBulletButton, smokeGButton, waterCButton, reinforcementButton);

        if (smokeGrenadeCoolDown)
        {
            smokeGButtonImg.fillAmount -= 1 / smokeGTimer * Time.deltaTime;
            if (smokeGButtonImg.fillAmount <= 0)
            {
                smokeGButtonImg.fillAmount = 1;
                smokeGrenadeCoolDown = false;
            }
        }

        if (waterCannonCoolDown)
        {
            waterCButtonImg.fillAmount -= 1 / waterCTimer * Time.deltaTime;
            if (waterCButtonImg.fillAmount <= 0)
            {
                waterCButtonImg.fillAmount = 1;
                waterCannonCoolDown = false;
            }
        }

        if (reinforceCoolDown)
        {
            reinforceButtonImg.fillAmount -= 1 / reinforceTimer * Time.deltaTime;
            if (reinforceButtonImg.fillAmount <= 0)
            {
                reinforceButtonImg.fillAmount = 1;
                reinforceCoolDown = false;
            }
        }
    }


    public void OnArrestButtonClicked()
    {
        //Logic of arrest selected for police
        foreach (PoliceAbilities pA in policeSquad)
            pA.Arrest();
    }
    
    public void OnAggressiveArrestButtonClicked()
    {
        //Logic of aggressive arrest selected for police
        foreach (PoliceAbilities pA in policeSquad)
            pA.AggressiveArrest();
    }

    public void OnRubberBulletsButtonClicked()
    {
        //check to make sure only one bullet is selected at a time
        if (!rubberBulletsSelected)
        {
            rubberBulletsSelected = true;
            lethalBulletsSelected = false;
        }

        //Logic of rubber bullets selected for police
        foreach (PoliceAbilities pA in policeSquad)
            pA.UseRubberBullets();
    }
    
    public void OnFireBulletsButtonClicked()
    {
        //Logic of rubber bullets selected for police
        foreach (PoliceAbilities pA in policeSquad)
            pA.FireBullets();
    }
    public void OnSmokeGrenadeButtonClicked()
    {
        //Logic of smoke grenade selected for police
        if (!smokeGrenadeCoolDown)
        {
            foreach (PoliceAbilities pA in policeSquad)
                pA.UseSmokeGrenade();
            smokeGrenadeCoolDown = true;
        }
    }
    public void OnWaterCannonButtonClicked()
    {
        //Logic of water cannon selected for police
        if (!waterCannonCoolDown)
        {
            foreach (PoliceAbilities pA in policeSquad)
                pA.UseWaterCannon();
            waterCannonCoolDown = true;
        }
    }
    public void OnLethalBulletsButtonClicked()
    {
        //check to make sure only one bullet is selected at a time
        if (!lethalBulletsSelected)
        {
            rubberBulletsSelected = false;
            lethalBulletsSelected = true;
        }

        //Logic of lethal force selected for police
        foreach (PoliceAbilities pA in policeSquad)
            pA.UseLethalBullets();
    }
    
    public void OnReinforceSquadButtonClicked()
    {
        //Logic of reinforce selected for police
        if (!reinforceCoolDown)
        {
            foreach (PoliceAbilities pA in policeSquad)
            {
                pA.ReinforceSquad();
                break;
            }
            reinforceCoolDown = true;
        }
    }

    //Helper function checks whether the police squad has enough funding to use abilities with a cost
    private void CheckIfEnoughFundingForButtons(Button type1, Button type2, Button type3, Button type4)
    {
        int totalFireBulletsCostAmongPoliceFlock = 0;
        int totalSmokeGCostAmongPoliceFlock = 0;
        int totalWaterCCostAmongPoliceFlock = 0;

        foreach (PoliceAbilities pA in policeSquad)
        {
            totalFireBulletsCostAmongPoliceFlock += pA.CostForFireBullets;
            totalSmokeGCostAmongPoliceFlock += pA.CostForSmokeGrenade;
            totalWaterCCostAmongPoliceFlock += pA.CostForWaterCannon;
        }

        if (totalFireBulletsCostAmongPoliceFlock > publicRelations._PRaccumulated)
            type1.interactable = false;
        else
            type1.interactable = true;
        
        if (totalSmokeGCostAmongPoliceFlock > publicRelations._PRaccumulated)
            type2.interactable = false;
        else
            type2.interactable = true;
        
        if (totalWaterCCostAmongPoliceFlock > publicRelations._PRaccumulated)
            type3.interactable = false;
        else
            type3.interactable = true;
        
        if (policeSquad[0].CostForReinforcement > publicRelations._PRaccumulated)
            type4.interactable = false;
        else
            type4.interactable = true;

    }

    //Helper function for checking which bullets are selected
    private void CheckWhaBulletIsSelected(Button rb, Button lb)
    {
        if (rubberBulletsSelected)
            rb.interactable = false;
        else
            rb.interactable = true;


        if (lethalBulletsSelected)
            lb.interactable = false;
        else
            lb.interactable = true;
    }
}
