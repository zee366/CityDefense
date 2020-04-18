using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    /// <summary>
    /// Handle the spawn probability an distribution.
    /// Spawns proper Prefab
    /// </summary>
    public class Spawner : MonoBehaviour {

        public  List<Spawnable> spawnables;
        public  Transform       spawnParent;

        public float spawnPerSecond = 1f;
        public int maxNumberOfEntities = 250;

        List<Vector2> probabilitiesOfSpawnableObjects = new List<Vector2>();

        /// <summary>
        /// Run spawn lotery to spawn one of the spawnable
        /// </summary>
        public void SpawnOne() {
            // Create a random number between 0 and 100
            float RandomProbability = Random.Range(0.0f, 100.0f);

            // Create an object type based on probability range it lies within
            int i = 0;
            foreach ( Vector2 prob in probabilitiesOfSpawnableObjects ) {
                if ( RandomProbability >= prob.x && RandomProbability <= prob.y ) {
                    Instantiate(spawnables[i].prefab, transform.position, Quaternion.identity, spawnParent);
                    break;
                }

                i++;
            }
        }

        private void Start() {
            // Initialize the list of probabilities with the ranges of probabilities 
            float startingProb = 0.0f;
            foreach ( Spawnable s in spawnables ) {
                float endingProb = startingProb + s.probability;
                probabilitiesOfSpawnableObjects.Add(new Vector2(startingProb, endingProb));
                startingProb += s.probability;
            }

            StartCoroutine(SpawnUntilFull());
        }


        private IEnumerator SpawnUntilFull() {
            while ( true ) {
                if(spawnParent.childCount < maxNumberOfEntities)
                    SpawnOne();

                yield return new WaitForSeconds(spawnPerSecond);
            }
        }

    }
}