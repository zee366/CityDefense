using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour {

    public List<Roadlight> OneDirectionPost;
    public List<Roadlight> TheOtherDirectionPost;

    private Coroutine _coroutine;
    private float _timer = 2f;
    private float _timer2 = 2f;
    // Start is called before the first frame update
    void Start() {
        _timer = Random.Range(2, 6);
        _timer2 = Random.Range(2, 6);
        _coroutine = StartCoroutine("Run");
    }

    IEnumerator Run() {
        while(true) {
            // Debug.Log("switch");
            OneDirectionPost.ForEach(r =>r.turnOnGreenLight() );
            TheOtherDirectionPost.ForEach(r =>r.turnOnRedLight() );
            yield return new WaitForSeconds(_timer);
            TheOtherDirectionPost.ForEach(r =>r.turnOnGreenLight() );
            OneDirectionPost.ForEach(r =>r.turnOnRedLight() );
            yield return new WaitForSeconds(_timer2);
        }
    }
    
    
}
