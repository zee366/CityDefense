using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicRelations : MonoBehaviour
{
    private int _PRaccumulated = 0;
    private int _PRrate = 15;
    private int _PRlevel = 50; 
    public Text Level_text;
    public Text Accumulated_text;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("PRTick", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevelRate();
        Accumulated_text.text = "$"+_PRaccumulated.ToString();
    }

    private void UpdateLevelRate()
    {
        if (_PRlevel >= 100)
        {
            _PRrate = 25;
            Level_text.text = "Best ($" + _PRrate + "/s)";
        }
        else if (_PRlevel >= 75)
        {
            _PRrate = 20;
            Level_text.text = "Good ($" + _PRrate + "/s)";
        }
        else if (_PRlevel >= 50)
        {
            _PRrate = 15;
            Level_text.text = "Neutral ($" + _PRrate + "/s)";
        }
        else if (_PRlevel >= 25)
        {
            _PRrate = 10;
            Level_text.text = "Bad ($" + _PRrate + "/s)";
        }
        else
        {
            _PRrate = 5;
            Level_text.text = "Worst ($" + _PRrate + "/s)";
        }
    }

    public void ImprovePR(int added)
    {
        _PRlevel = Mathf.Clamp(_PRlevel + added, -25, 125);
    }
    
    public void WorsenPR(int removed)
    {
        _PRlevel = Mathf.Clamp(_PRlevel - removed, -25, 125);
    }

    public void CostOfAction(int cost)
    {
        _PRaccumulated -= cost;
    }

    private void PRTick()
    {
        _PRaccumulated += _PRrate;
    }
}
