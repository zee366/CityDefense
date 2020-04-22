using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Surveillance : MonoBehaviour
{
    public Image surveillanceImg;

    private PublicRelations publicRelations;
    
    private enum SurveillanceLevel { Level_0, Level_1, Level_2, Level_3};
    private int[] costs = new int[3] { 200,400,600};
    private SurveillanceLevel current = SurveillanceLevel.Level_0; 

    void Start()
    {
        publicRelations = FindObjectOfType<PublicRelations>();
        Debug.Log(costs[0]);
        Debug.Log(costs[1]);
        Debug.Log(costs[2]);

    }

    void Update()
    {
        switch (current)
        {
            case SurveillanceLevel.Level_0:
                break;
            case SurveillanceLevel.Level_1:
                break;
            case SurveillanceLevel.Level_2:
                break;
            case SurveillanceLevel.Level_3:
                break;
        }
    }
}
