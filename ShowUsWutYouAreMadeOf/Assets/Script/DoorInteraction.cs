using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class DoorInteraction : Interactable
{
    [SerializeField] Vector3 indoorLandingPosition, outdoorLandingPosition;
    GameObject player;
    CharacterController controller;
    public static bool indoor;

    protected override void Start()
    {
        base.Start();
        player = gm.player;
        controller = player.GetComponent<CharacterController>();
        indoor = gm.startIndoor;
        ModifyWardrobeAndSound();
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
        controller.enabled = true;
        indoor = !indoor;
        ModifyWardrobeAndSound();

        StartCoroutine(InteractionCooldown(1f));
        indicator.DisplayIndicator(false);
    }

    void ModifyWardrobeAndSound()
    {
        gm.EnableWardrobeAction(indoor);
        AudioManager.Instance.SetMuffleParameter(indoor? 0f: 1f);
    }
}
