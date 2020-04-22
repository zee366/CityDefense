using FluidHTN;
using Rioters;
using UnityEngine;

public class RioterHealth : MonoBehaviour
{
    public float health;

    public bool IsDead { get; set; }

    public NpcHtnContext Context { get; private set; }

    void Awake()
    {
        IsDead = false;
        health = 100.0f;
    }

    public void Init(NpcHtnContext context)
    {
        Context = context;
    }

    public void TakeDamage(float amount)
    {
        if (health <= 0.01f)
        {
            //Debug.Log("Rioter health to set has died");
            health = 0.0f;
            IsDead = true; 
            Context.SetState(NpcWorldState.HasDied, true, EffectType.Permanent);
        }
        else
        {
            health -= amount;
            Debug.Log("in rioter taking damage");
            Context.SetState(NpcWorldState.HasTakenDamage, true, EffectType.Permanent);
        }
    }

    //Function used at end of death animation
    public void Death()
    {
        Destroy(gameObject);
    }
}
