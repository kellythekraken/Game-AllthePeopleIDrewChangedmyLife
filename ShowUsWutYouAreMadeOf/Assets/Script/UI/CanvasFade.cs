using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script that controls the alpha (transparency) of a CanvasGroup, and
// can animate a fade in and fade out.

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFade : MonoBehaviour
{
    private CanvasGroup canvasGroup; // canvas UI layer we are fading

    // when this fade layer object is created
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void BlockRayCast(bool block)
    {
        canvasGroup.blocksRaycasts = block;
    }
    // A coroutine that fades to transparency {alpha} over {time} seconds
    public IEnumerator ChangeAlphaOverTime(float startAlpha, float endAlpha, float time) {
        float elapsed = 0f;
        BlockRayCast(true); 
        while (elapsed < time) {
            var factor = elapsed / time;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, factor);
            yield return null;
            elapsed += Time.deltaTime;
        }
        BlockRayCast(endAlpha!=0);

        canvasGroup.alpha = endAlpha;
    }
}
