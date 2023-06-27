using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FootstepReceiver : MonoBehaviour
{
    AudioManager aM;
    EventReference stepEvent;

    void Start()
    {
        aM = AudioManager.Instance;
        stepEvent = FMODEvents.Instance.footStep;

    }
    private void OnFootstep() => aM.PlayOneShot(stepEvent);
}
