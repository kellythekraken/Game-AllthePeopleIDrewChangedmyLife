using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : Interactable
{
    [SerializeField] Vector3 indoorLandingPosition;
    public Transform player;
    
    protected override void OnTriggerEnter(Collider other)
    {
        //display name (either enter/exit)
        if (!other.CompareTag("Player") || !interactable) return;
        InRange = true;
        indicator.ChangeText("Enter"); 
    }

    //when action key is pressed
    protected override void StartInteraction()
    {
        TeleportToIndoor();
    }


    void TeleportToIndoor()
    {
       Debug.Log("teleport indoor!");
       player.transform.position = indoorLandingPosition;
    }
}
