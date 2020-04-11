using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPoliceAbilities : MonoBehaviour
{
    public PoliceAbilities policeSquadLeader;

    
    float smokeGTimer;
    float smokeGTimerDelay;
    bool smokeGrenadeCoolDown;

    float waterCTimer;
    float waterCTimerDelay;
    bool waterCannonCoolDown;

    float reinforceTimer;
    float reinforceTimerDelay;
    bool reinforceCoolDown;



    // Start is called before the first frame update
    void Start()
    {
        smokeGTimer = 2.0f;
        smokeGTimerDelay = 2.0f;
        smokeGrenadeCoolDown = false;

        waterCTimer = 3.0f;
        waterCTimerDelay = 3.0f;
        waterCannonCoolDown = false;

        reinforceTimer = 2.0f;
        reinforceTimerDelay = 2.0f;
        reinforceCoolDown = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (smokeGrenadeCoolDown)
        {
            smokeGTimer -= Time.deltaTime;
            if (smokeGTimer <= 0)
            {
                smokeGTimer = smokeGTimerDelay;
                smokeGrenadeCoolDown = false;
            }
        }

        if (waterCannonCoolDown)
        {
            waterCTimer -= Time.deltaTime;
            if (waterCTimer <= 0)
            {
                waterCTimer = waterCTimerDelay;
                waterCannonCoolDown = false;
            }
        }

        if (reinforceCoolDown)
        {
            reinforceTimer -= Time.deltaTime;
            if (reinforceTimer <= 0)
            {
                reinforceTimer = reinforceTimerDelay;
                reinforceCoolDown = false;
            }
        }
    }


    public void OnArrestButtonClicked()
    {
        //Logic of arrest selected for police
        policeSquadLeader.Arrest();
    }
    
    public void OnAggressiveArrestButtonClicked()
    {
        //Logic of aggressive arrest selected for police
        policeSquadLeader.AggressiveArrest();
    }

    public void OnRubberBulletsButtonClicked()
    {
        //Logic of rubber bullets selected for police
        policeSquadLeader.UseRubberBullets();
    }
    public void OnSmokeGrendadeButtonClicked()
    {
        //Logic of smoke grenade selected for police
        if (!smokeGrenadeCoolDown)
        {
            policeSquadLeader.UseSmokeGrenade();
            smokeGrenadeCoolDown = true;
        }
    }
    public void OnWaterCannonButtonClicked()
    {
        //Logic of water cannon selected for police
        if (!waterCannonCoolDown)
        {
            policeSquadLeader.UseWaterCannon();
            waterCannonCoolDown = true;
        }
    }
    public void OnLethalForceButtonClicked()
    {
        //Logic of lethal force selected for police
        policeSquadLeader.UseLethalBullets();
    }
    
    public void OnReinforceSquadButtonClicked()
    {
        //Logic of reinforce selected for police
        if (!reinforceCoolDown)
        {
            policeSquadLeader.ReinforceSquad();
            reinforceCoolDown = true;
        }
    }
}
