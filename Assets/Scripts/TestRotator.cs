using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotator : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter)) {
            Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);
            StartCoroutine(Rotate(rotation));
        }
    }

    IEnumerator Rotate(Quaternion rotation) {
        while(transform.rotation != rotation) {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}
