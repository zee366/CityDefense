using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDamageFX : MonoBehaviour
{
    public float damage;
    public Destructible target;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            target.TakeDamage(damage);
            //if(!GameController.instance.gamePaused)
            //    GameController.instance.DamageCity(damage);
        }

        if(Input.GetKeyDown(KeyCode.Z))
            GameController.instance.PauseGame();
    }
}
