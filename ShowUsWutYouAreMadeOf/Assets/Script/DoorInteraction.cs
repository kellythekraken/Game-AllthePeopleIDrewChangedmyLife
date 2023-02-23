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
   /* protected override void OnTriggerEnter(Collider other)
    {
        //display name (either enter/exit)
        if (!other.CompareTag("Player") || !interactable) return;
        indicator.ChangeText(indoor? "Exit" :"Enter"); 
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || !interactable) return;
        indicator.DrawRay();//"Door"
    }*/

    //when action key is pressed
    public override void StartInteraction()
    {
        Teleport();
    }


    void Teleport()
    {
        controller.enabled = false;
        player.transform.position = indoor? outdoorLandingPosition : indoorLandingPosition;
        AudioManager.Instance.SetMuffleParameter(indoor? 0f: 1f);
        controller.enabled = true;
        indoor = !indoor;
        
        StartCoroutine(InteractionCooldown(1f));
        indicator.DisplayIndicator(false);
    }
}
