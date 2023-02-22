using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class DoorInteraction : Interactable
{
    [SerializeField] Vector3 indoorLandingPosition;
    GameObject player;
    ThirdPersonController controller;

    protected override void Start()
    {
        base.Start();
        player = gm.player;
        controller = player.GetComponent<ThirdPersonController>();
    }
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
//       controller.stopMoveUpdate = true;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = indoorLandingPosition;
        player.GetComponent<CharacterController>().enabled = true;

    }
}
