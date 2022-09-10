using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueRange : MonoBehaviour
{
    internal bool InRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) InRange = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) InRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) InRange = false;
    }
}
