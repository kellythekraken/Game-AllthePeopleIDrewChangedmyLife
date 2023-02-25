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
    public override string GetName() 
     {
        return indoor? "Exit" :"Enter"; 

     }

    //when action key is pressed
    public override void StartInteraction()
    {
        if(interactable) Teleport();
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
