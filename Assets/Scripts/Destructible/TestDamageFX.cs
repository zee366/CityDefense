using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDamageFX : MonoBehaviour
{
    public float damage;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(!GameController.instance.gamePaused)
                GameController.instance.DamageCity(damage);
        }

        if(Input.GetKeyDown(KeyCode.Z))
            GameController.instance.PauseGame();
    }
}
