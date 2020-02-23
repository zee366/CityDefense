using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

namespace Utils {
    /// <summary>
    /// Handle the spawn probability an distribution.
    /// Spawns proper Prefab
    /// </summary>
    public class Spawner : MonoBehaviour {

        public  List<Spawnable> spawnables;
        public  Transform       spawnParent;
        private SplineNode      _firstNode;

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
                    Instantiate(spawnables[i].prefab, new Vector3(0.0f, -100.0f, 0.0f), Quaternion.identity, spawnParent);
                    break;
                }

                i++;
            }
        }

        /// <summary>
        /// Instantiate Boss
        /// </summary>
        
        //public void SpawnBoss() {
        //    Instantiate(spawnables[spawnables.Count - 1].prefab, _firstNode.Position, Quaternion.identity, spawnParent);
        //}

        private void Start() {
            // Initialize the list of probabilities with the ranges of probabilities 
            float startingProb = 0.0f;
            foreach ( Spawnable s in spawnables ) {
                float endingProb = startingProb + s.probability;
                probabilitiesOfSpawnableObjects.Add(new Vector2(startingProb, endingProb));
                startingProb += s.probability;
            }

            //_firstNode = spawnParent.GetComponent<Spline>().nodes[0];
        }

    }
}