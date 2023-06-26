using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class DoorInteraction : Interactable
{
    public static DoorInteraction Instance;
    [SerializeField] Vector3 indoorLandingPosition, outdoorLandingPosition;
    GameObject player;
    PlayerTeleport teleportScript;
    bool _indoor;
    internal bool indoor {get{ return _indoor;} set {_indoor = value; ModifyWardrobeAndSound();}}
    bool firstEntrance = false;

    void Awake() => Instance = this;
    protected override void Start()
    {
        base.Start();

        player = gm.player;
        teleportScript = player.GetComponent<PlayerTeleport>();
    }
    public override string GetName() 
     {
        return indoor? "Exit" :"Enter"; 
     }

    //when action key is pressed
    public override void StartInteraction()
    {
        if(!firstEntrance) StartCoroutine(FirstEntranceTeleport());
        else if(interactable) Teleport();
    }
    IEnumerator FirstEntranceTeleport()
    {
        gm.FadeIn();
        firstEntrance = true;
        //teleport to leon and trigger dialogue
        yield return new WaitForSeconds(.6f);
        teleportScript.TeleportToLeon();
        NPCManager.Instance.FindNPC("Leon").StartInteraction();
        indoor = true;
        gm.FadeOut();
    }
    void Teleport()
    {
        gm.FadeIn();
        teleportScript.Teleport(indoor? outdoorLandingPosition : indoorLandingPosition);
        indoor = !indoor;
        gm.FadeOut();

        StartCoroutine(InteractionCooldown(1f));
        indicator.DisplayIndicator(false);
    }

    void ModifyWardrobeAndSound()
    {
        if(gm == null) gm = GameManager.Instance;
        gm.EnableWardrobeAction(indoor);
        AudioManager.Instance.SetMuffleParameter(indoor? 0f: 1f);
    }
}
