using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicRelations : MonoBehaviour
{
    public int _PRaccumulated = 0;
    private int _PRrate = 15;
    private int _PRlevel = 50; 
    public Text Level_text;
    public Text Accumulated_text;

    public int Best = 100;
    public int Good = 50;
    public int Neutral = 25;
    public int Bad = 20;
    public int Worst = 15; 

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
            _PRrate = Best;
            Level_text.text = "Best ($" + _PRrate + "/s)";
        }
        else if (_PRlevel >= 75)
        {
            _PRrate = Good;
            Level_text.text = "Good ($" + _PRrate + "/s)";
        }
        else if (_PRlevel >= 50)
        {
            _PRrate = Neutral;
            Level_text.text = "Neutral ($" + _PRrate + "/s)";
        }
        else if (_PRlevel >= 25)
        {
            _PRrate = Bad;
            Level_text.text = "Bad ($" + _PRrate + "/s)";
        }
        else
        {
            _PRrate = Worst;
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
