using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPoliceAbilities : MonoBehaviour
{
    public PoliceAbilities policeSquadLeader;
    //public GameObject policeSquadLeader;

    public Image smokeGButton;
    float smokeGTimer;
    bool smokeGrenadeCoolDown;

    public Image waterCButton;
    float waterCTimer;
    bool waterCannonCoolDown;

    public Image reinforceButton;
    float reinforceTimer;
    bool reinforceCoolDown;



    // Start is called before the first frame update
    void Start()
    {
        policeSquadLeader = GetChildWithName(GameObject.Find("PoliceFlock"), "Agent 0").GetComponent<PoliceAbilities>();

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
        if (smokeGrenadeCoolDown)
        {
            smokeGButton.fillAmount -= 1 / smokeGTimer * Time.deltaTime;
            if (smokeGButton.fillAmount <= 0)
            {
                smokeGButton.fillAmount = 1;
                smokeGrenadeCoolDown = false;
            }
        }

        if (waterCannonCoolDown)
        {
            waterCButton.fillAmount -= 1 / waterCTimer * Time.deltaTime;
            if (waterCButton.fillAmount <= 0)
            {
                waterCButton.fillAmount = 1;
                waterCannonCoolDown = false;
            }
        }

        if (reinforceCoolDown)
        {
            reinforceButton.fillAmount -= 1 / reinforceTimer * Time.deltaTime;
            if (reinforceButton.fillAmount <= 0)
            {
                reinforceButton.fillAmount = 1;
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
