using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration;
    public bool delayStart;

    void Start()
    {
        if(delayStart)
            Invoke("StartFade", 5f);
    }

    public void StartFade() {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(Fade(canvasGroup));
    }

    IEnumerator Fade(CanvasGroup canvasGroup) {
        float counter = 0f;
        float start = 0f;
        float end = 1f;

        while(counter < fadeDuration) {
            counter += Time.fixedDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / fadeDuration);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}
