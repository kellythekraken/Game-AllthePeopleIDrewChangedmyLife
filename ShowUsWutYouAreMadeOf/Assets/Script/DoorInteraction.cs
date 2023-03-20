using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class DoorInteraction : Interactable
{
    [SerializeField] Vector3 indoorLandingPosition, outdoorLandingPosition;
    GameObject player;
    PlayerTeleport teleportScript;
    public static bool indoor;
    bool firstEntrance = false;

    protected override void Start()
    {
        base.Start();
        player = gm.player;
        teleportScript = player.GetComponent<PlayerTeleport>();
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
        if(firstEntrance) 
        { FirstEntranceTeleport(); return;}

        if(interactable) Teleport();
    }

    void FirstEntranceTeleport()
    {
        //teleport to leon for the cinematic start
        firstEntrance = true;
    }
    void Teleport()
    {
        teleportScript.Teleport(indoor? outdoorLandingPosition : indoorLandingPosition);
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
