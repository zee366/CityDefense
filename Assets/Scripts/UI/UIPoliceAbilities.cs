﻿using System.Collections;
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
    }

    // Update is called once per frame
    void Update()
    {
        //Only needed if rioters use Flock too...
        //if ( policeSquadLeader == null ) {
        //    GameObject go = GetChildWithName(GameObject.Find("PoliceFlock"), "Agent 0");
        //    if(go != null)
        //        policeSquadLeader = go.GetComponent<PoliceAbilities>();
        //}

        if (policeSquad.Count != flock.agents.Count)
        {
            foreach (FlockAgent fA in flock.agents)
            {
                PoliceAbilities pA = fA.gameObject.GetComponent<PoliceAbilities>();
                if (!policeSquad.Contains(pA))
                    policeSquad.Add(fA.gameObject.GetComponent<PoliceAbilities>());
            }
        }

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
    public void OnSmokeGrendadeButtonClicked()
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
    public void OnLethalForceButtonClicked()
    {
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

    //Function checks whether the police squad has enough funding to use abilities with a cost
    public void CheckIfEnoughFundingForButtons(Button type1, Button type2, Button type3, Button type4)
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

    //Not used, but will keep for now in case of rioters flock interfering with police flock
    GameObject GetChildWithName(GameObject obj, string name)
    {
        // Source: http://answers.unity.com/answers/1355797/view.html

        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }
}
