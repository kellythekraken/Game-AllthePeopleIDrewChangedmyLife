using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerFMODEvent : MonoBehaviour
{

    void Start()
    {
    }
      private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered!");
        if(other.CompareTag("Player"))
        {
            AudioManager.Instance.SetGlobalParameter("Muffle",0f);
        }
    }
}
