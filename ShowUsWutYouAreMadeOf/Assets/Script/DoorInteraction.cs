using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class DoorInteraction : Interactable
{
    [SerializeField] Vector3 indoorLandingPosition, outdoorLandingPosition;
    GameObject player;
    CharacterController controller;
    bool indoor = false;

    protected override void Start()
    {
        base.Start();
        player = gm.player;
        controller = player.GetComponent<CharacterController>();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        //display name (either enter/exit)
        if (!other.CompareTag("Player") || !interactable) return;
        InRange = true;
        indicator.ChangeText(indoor? "Exit" :"Enter"); 
    }

    //when action key is pressed
    protected override void StartInteraction()
    {
        Teleport();
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || !interactable) return;
        indicator.DrawRay("Door");
    }
    void Teleport()
    {
        Debug.Log("teleport indoor!");
//       controller.stopMoveUpdate = true;
        controller.enabled = false;
        player.transform.position = indoor? outdoorLandingPosition : indoorLandingPosition;
        AudioManager.Instance.SetMuffleParameter(indoor? 0f: 1f);
        controller.enabled = true;
        indoor = !indoor;
    }
}
