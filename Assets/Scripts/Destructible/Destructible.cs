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

    [Tooltip("Array of particle system groups that will become enabled as the object takes more and more damage")]
    public GameObject[] DamagedFX;

    [Tooltip("The final particle effect group to play during the death animation")]
    public GameObject DeathFX;

    // The starting level of destruction (100 = not damaged)
    private float _destructionLevel = 100f;

    // Indices into DamagedFX array
    private int _prevParticleSystem = -1;
    private int _nextParticleSystem = 0;

    // Verify that health has dropped into the next tier of graphical destruction effects
    private void CheckParticleFX() {
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

    // Grab all the particle systems in the next group and play them
    private void PlayNextParticleSystem() {
        if(_nextParticleSystem < DamagedFX.Length) {
            if(_prevParticleSystem >= 0) {
                ParticleSystem[] stopGroup = DamagedFX[_prevParticleSystem].GetComponentsInChildren<ParticleSystem>();
                foreach(ParticleSystem ps in stopGroup)
                    ps.Stop();

            }

            ParticleSystem[] startGroup = DamagedFX[_nextParticleSystem].GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem ps in startGroup)
                ps.Play();

            _prevParticleSystem = _nextParticleSystem++;
        }
    }

    // Deal damage to the object, verify if we need to play the next special effect, die if health is 0 or less
    public void TakeDamage(float damage) {
        health -= damage;
        CheckParticleFX();

        if(health <= 0f)
            Die();
    }

    // Sink into the ground over time. Play the final dying special effect
    private IEnumerator Sink() {
        if(_prevParticleSystem >= 0) {
            ParticleSystem[] stopGroup = DamagedFX[_prevParticleSystem].GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem ps in stopGroup)
                ps.Stop();
        }

        ParticleSystem[] startGroup = DeathFX.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in startGroup)
            ps.Play();

        while(true) {
            Vector3 movement = Vector3.down * sinkSpeed * Time.deltaTime;
            transform.position += movement;

            DeathFX.transform.position -= movement;

            destructionTime -= Time.deltaTime;
            if(destructionTime <= 0f) {
                Destroy(gameObject);
                break;
            }
            yield return null;
        }
    }

    // Called when health drops to 0 or less
    private void Die() {
        StartCoroutine(Sink());
    }
}
