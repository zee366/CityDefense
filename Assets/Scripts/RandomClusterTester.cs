using System.Collections;
using UnityEngine;

public class RandomClusterTester : MonoBehaviour {

    public DynamicClustersApproximator approximator;
    public int testCount = 100;

    public Mesh mesh;

    void Awake() {

        // Populate with randomly positioned objects
        for ( int i = 0; i < testCount; i++ ) {
            var go = new GameObject();
            go.transform.position = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
            go.transform.SetParent(approximator.transform);

            // Render something
            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshRenderer>();
        }

        StartCoroutine(TestRoutine());
    }


    private IEnumerator TestRoutine() {
        while ( true ) {
            // Query from random object to query from.
            int i = Random.Range(0, testCount);
            Vector3 queryPos = approximator.transform.GetChild(i).position;
            Debug.DrawLine(queryPos, queryPos + Vector3.up * 20, Color.green, 1f);

            Vector3 targetResult = approximator.GetClosestApproximatedClusterPosition(queryPos);
            Debug.DrawLine(targetResult, targetResult + Vector3.up * 20, Color.red, 1f);

            yield return new WaitForSeconds(1f);
        }
    }

}