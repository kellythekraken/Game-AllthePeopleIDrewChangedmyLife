using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field:Header("Ambience")]
    [field:SerializeField] public EventReference chatter {get ; private set;}
    [field:SerializeField] public EventReference music {get ; private set;}

    
    [field:Header("SFX")]
    [field:SerializeField] public EventReference footStep {get ; private set;}
    [field:SerializeField] public EventReference bookOpen {get ; private set;}
    [field:SerializeField] public EventReference bookClose {get ; private set;}
    [field:SerializeField] public EventReference curtain {get ; private set;}
    [field:SerializeField] public EventReference draw {get ; private set;}
    [field:SerializeField] public EventReference pen_pickup {get ; private set;}
    [field:SerializeField] public EventReference bartend_pour {get ; private set;}
    [field:SerializeField] public EventReference bartend_shake {get ; private set;}
    public static FMODEvents Instance { get; private set;}

    void Awake()
    {
        Instance = this;
    }


}
