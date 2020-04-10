using System.Collections;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Tooltip("Current health of the object")]
    public float health;

    [Tooltip("Time for which the death animation will play after reaching 0 health")]
    public float destructionTime;

    [Tooltip("Speed at which the object sinks into the ground during the death animation")]
    public float sinkSpeed;

    [Tooltip("The interval separating various levels of destruction")]
    public float destructionStep;

    [Tooltip("Array of particle systems that will become emabled as the object takes more and more damage")]
    public ParticleSystem[] DamagedFX;

    [Tooltip("The final particle effect to play during the death animation")]
    public ParticleSystem DeathFX;

    // The starting level of destruction (100 = not damaged)
    private float _destructionLevel = 100f;

    // Index into DamagedFX array
    private int _nextParticleSystem = 0;

    public void CheckParticleFX() {
        for(int i = _nextParticleSystem; i < DamagedFX.Length; ++i) {
            if(health < _destructionLevel) {
                PlayNextParticleSystem();
                _destructionLevel -= destructionStep;
            }
            else {
                return;
            }
        }
    }

    public void PlayNextParticleSystem() {
        if(_nextParticleSystem < DamagedFX.Length)
            DamagedFX[_nextParticleSystem++].Play();
    }

    public void TakeDamage(float damage) {
        health -= damage;
        CheckParticleFX();

        if(health <= 0f)
            Die();
    }

    IEnumerator Sink() {
        DeathFX.Play();
        while(true) {
            Vector3 movement = Vector3.down * sinkSpeed * Time.deltaTime;
            transform.position += movement;
            destructionTime -= Time.deltaTime;
            if(destructionTime <= 0f) {
                Destroy(gameObject);
                break;
            }
            yield return null;
        }
    }

    public void Die() {
        StartCoroutine(Sink());
    }
}
