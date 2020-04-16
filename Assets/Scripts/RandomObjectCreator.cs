using UnityEngine;

public class RandomObjectCreator : MonoBehaviour {

    public Transform spawnIn;
    public int testCount = 100;

    void Awake() {

        for ( int i = 0; i < testCount; i++ ) {
            var go = new GameObject();
            go.transform.position = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
            go.transform.SetParent(spawnIn);
        }

    }


}