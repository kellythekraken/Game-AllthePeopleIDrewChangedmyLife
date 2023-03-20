using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextModifier : MonoBehaviour
{
    public float delay = 0.1f;
    TextMeshProUGUI text;
    string currText = "";
    string fullText;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        fullText = text.text;
        StartCoroutine(Typewrite());
    }

    public IEnumerator Typewrite()
    {
        for(int i=0; i< fullText.Length + 1; i++)
        {
            currText = fullText.Substring(0,i);
            text.text = currText;
            yield return new WaitForSeconds(delay);
        }
    }

    public void UnderlineFont()
    {
        text.fontStyle = FontStyles.Underline;
    }

    public void ResetFontStyle()
    {
        text.fontStyle = FontStyles.Normal;
    }
}
