using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Surveillance : MonoBehaviour
{
    public Image surveillanceImg;
    public Button surveillanceButton;
    public PublicRelations publicRelations;
    public Camera miniMapCam;
    public enum SurveillanceLevel { Level_0, Level_1, Level_2, Level_3};
    public static SurveillanceLevel current = SurveillanceLevel.Level_0;
    public int Level1Cost = 200;
    public int Level2Cost = 400;
    public int Level3Cost = 800;
    public Text level_0;
    public Text level_1;
    public Text level_2;
    public Text level_3;
    //void Start()
    //{
    //    miniMapCam = GameObject.Find("Minimap Camera").GetComponent<Camera>();
    //    publicRelations = FindObjectOfType<PublicRelations>();
    //}

    void Update()
    {
        CheckCost();
    }

    public void CheckCost()
    {
        switch (current)
        {
            case SurveillanceLevel.Level_0:
                if (Level1Cost > publicRelations._PRaccumulated)
                    surveillanceButton.interactable = false;
                else
                    surveillanceButton.interactable = true;
                break;
            case SurveillanceLevel.Level_1:
                if (Level2Cost > publicRelations._PRaccumulated)
                    surveillanceButton.interactable = false;
                else
                    surveillanceButton.interactable = true;
                break;
            case SurveillanceLevel.Level_2:
                if (Level3Cost > publicRelations._PRaccumulated)
                    surveillanceButton.interactable = false;
                else
                    surveillanceButton.interactable = true;
                break;
            case SurveillanceLevel.Level_3:
                surveillanceButton.interactable = false;
                break;
        }
    }

    public void OnSurveillanceButtonClicked()
    {
        switch (current)
        {
            case SurveillanceLevel.Level_0:
                current = SurveillanceLevel.Level_1;
                level_0.enabled = false;
                level_1.enabled = true;
                miniMapCam.cullingMask |= 1 << LayerMask.NameToLayer("Reporter");
                publicRelations.CostOfAction(Level1Cost);

                break;
            case SurveillanceLevel.Level_1:
                current = SurveillanceLevel.Level_2;
                level_1.enabled = false;
                level_2.enabled = true;
                miniMapCam.cullingMask |= 1 << LayerMask.NameToLayer("BuildingDmg");
                publicRelations.CostOfAction(Level2Cost);
                break;
            case SurveillanceLevel.Level_2:
                current = SurveillanceLevel.Level_3;
                level_2.enabled = false;
                level_3.enabled = true;
                miniMapCam.cullingMask |= 1 << LayerMask.NameToLayer("Rioters");
                publicRelations.CostOfAction(Level3Cost);
                break;

        }
    }
}
