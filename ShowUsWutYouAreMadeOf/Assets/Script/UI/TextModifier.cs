using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextModifier : MonoBehaviour
{
    public float delay = 0.07f;
    TextMeshProUGUI text;
    string currText = "";
    string fullText;
    internal bool typing = false;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        fullText = text.text;
        ClearText();
    }

    public IEnumerator Typewrite()
    {
        typing = true;
        for(int i=0; i< fullText.Length + 1; i++)
        {
            currText = fullText.Substring(0,i);
            text.text = currText;
            yield return new WaitForSeconds(delay);
        }
        typing = false;
    }
    public void ClearText() => text.text = "";
}
