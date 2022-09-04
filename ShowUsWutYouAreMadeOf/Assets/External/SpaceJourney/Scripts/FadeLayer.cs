using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// YOU WILL NOT HAVE TO CHANGE THIS FILE
// this file is attached to the FadeLayer in the Scene and its functions only
// exist for functions in SceneDirector.cs to call them
// fades CanvasGroup to transparency {alpha} over {time} seconds


// A script that controls the alpha (transparency) of a CanvasGroup, and
// can animate a fade in and fade out.
[RequireComponent(typeof(CanvasGroup))]
public class FadeLayer : MonoBehaviour
{
    // Whether we start the scene fully faded out (i.e. 100% black) or not
    [SerializeField] bool startFadedOut = false;
    private CanvasGroup canvasGroup; // canvas UI layer we are fading

    // when this fade layer object is created
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = startFadedOut ? 1 : 0;
    }

    // A coroutine that fades to transparency {alpha} over {time} seconds
    public IEnumerator ChangeAlphaOverTime(float alpha, float time) {
        float currentAlpha = canvasGroup.alpha;
        float elapsed = 0f;
        while (elapsed < time) {
            var factor = elapsed / time;
            canvasGroup.alpha = Mathf.Lerp(currentAlpha, alpha, factor);
            yield return null;
            elapsed += Time.deltaTime;
        }
        canvasGroup.alpha = alpha;
    }
}
