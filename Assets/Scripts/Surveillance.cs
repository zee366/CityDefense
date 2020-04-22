using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Surveillance : MonoBehaviour
{
    public Image surveillanceImg;
    public Button surveillanceButton;
    private PublicRelations publicRelations;
    private Camera miniMapCam;
    private enum SurveillanceLevel { Level_0, Level_1, Level_2, Level_3};
    private Dictionary<SurveillanceLevel,int> costs;
    private SurveillanceLevel current = SurveillanceLevel.Level_0;
    public int Level1Cost = 200;
    public int Level2Cost = 400;
    public int Level3Cost = 800;
    public Text level_0;
    public Text level_1;
    public Text level_2;
    public Text level_3;
    void Start()
    {
        miniMapCam = GameObject.Find("Minimap Camera").GetComponent<Camera>();
        publicRelations = FindObjectOfType<PublicRelations>();
        costs = new Dictionary<SurveillanceLevel, int>
        {
            { SurveillanceLevel.Level_0, 0 },
            { SurveillanceLevel.Level_1, Level1Cost },
            { SurveillanceLevel.Level_2, Level2Cost },
            { SurveillanceLevel.Level_3, Level3Cost }
        };

    }

    void Update()
    {
        CheckCost();
    }

    public void CheckCost()
    {
        switch (current)
        {
            case SurveillanceLevel.Level_0:
                if (costs[SurveillanceLevel.Level_1] > publicRelations._PRaccumulated)
                    surveillanceButton.interactable = false;
                else
                    surveillanceButton.interactable = true;
                break;
            case SurveillanceLevel.Level_1:
                if (costs[SurveillanceLevel.Level_2] > publicRelations._PRaccumulated)
                    surveillanceButton.interactable = false;
                else
                    surveillanceButton.interactable = true;
                break;
            case SurveillanceLevel.Level_2:
                if (costs[SurveillanceLevel.Level_3] > publicRelations._PRaccumulated)
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
                break;
            case SurveillanceLevel.Level_1:
                current = SurveillanceLevel.Level_2;
                level_1.enabled = false;
                level_2.enabled = true;
                miniMapCam.cullingMask |= 1 << LayerMask.NameToLayer("BuildingDmg");
                break;
            case SurveillanceLevel.Level_2:
                current = SurveillanceLevel.Level_3;
                level_2.enabled = false;
                level_3.enabled = true;
                miniMapCam.cullingMask |= 1 << LayerMask.NameToLayer("Rioters");
                break;

        }
    }
}
