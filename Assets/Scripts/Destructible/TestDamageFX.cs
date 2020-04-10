using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamageFX : MonoBehaviour
{
    public GameObject prefab;
    public float damage;
    private Destructible destructible;

    // Start is called before the first frame update
    void Start()
    {
        destructible = prefab.GetComponent<Destructible>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            destructible.TakeDamage(damage);
        }
    }
}
