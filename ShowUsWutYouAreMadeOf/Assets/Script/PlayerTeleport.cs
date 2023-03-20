using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//called by doorinteraction to control landing position
//also by gamemanager for start indoor/outdoor location

//maybe also for changing position when in a conversation??
public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] Vector3 indoorStartLocation, outdoorStartLocation;

    CharacterController controller;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    public void Teleport(Vector3 destination)
    {
        controller.enabled = false;
        transform.position = destination;
        controller.enabled = true;
    }

    public void TeleportToStartLocation(bool startindoor)
    {
        Teleport(startindoor? indoorStartLocation:outdoorStartLocation);
    }
}
